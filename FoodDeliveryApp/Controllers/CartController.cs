using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ILogger<CartController> _logger;

        public CartController(
            ICartService cartService,
            IMenuItemRepository menuItemRepository,
            ILogger<CartController> logger)
        {
            _cartService = cartService;
            _menuItemRepository = menuItemRepository;
            _logger = logger;
        }

        // GET: Cart/Index
        [HttpGet]
        [Route("cart")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var cart = await _cartService.GetCartAsync(User.Identity.Name);
                var cartItems = cart.Items.Select(item => new CartItemViewModel
                {
                    CartItemId = item.Id,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    Name = item.MenuItem.Name,
                    ImageUrl = item.MenuItem.ImageUrl ?? "/images/placeholder-dish.jpg",
                    Price = item.MenuItem.Price,
                    RestaurantId = item.MenuItem.RestaurantId,
                    RestaurantName = item.MenuItem.Restaurant.Name,
                    TaxRate = item.MenuItem.Restaurant.TaxRate,
                    DeliveryFee = item.MenuItem.Restaurant.DeliveryFee,
                    Customizations = item.Customizations.Select(c => new CustomizationViewModel
                    {
                        OptionId = c.OptionId,
                        ChoiceId = c.ChoiceId,
                        Price = c.Price
                    }).ToList()
                }).ToList();

                var viewModel = new CartViewModel
                {
                    Items = cartItems,
                    Subtotal = cartItems.Sum(item => (item.Price + item.Customizations.Sum(c => c.Price)) * item.Quantity),
                    DeliveryFee = CalculateDeliveryFee(cartItems),
                    Tax = CalculateTaxFee(cartItems),
                    Total = await _cartService.GetCartTotalAsync(User.Identity.Name) + CalculateDeliveryFee(cartItems) * (1 + 0.07m)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart for user {UserId}", User.Identity.Name);
                TempData["ErrorMessage"] = "Unable to load your cart. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("cart/add")]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartViewModel model)
        {
            if (model == null || (model.MenuItemId <= 0) || model.Quantity <= 0)
            {
                _logger.LogWarning("Invalid add to cart data for user {UserId}: {Model}", User.Identity.Name, model);
                return Json(new { success = false, message = "Invalid item data." });
            }

            try
            {
                var menuItem = await _menuItemRepository.GetByIdAsync(model.MenuItemId);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item {MenuItemId} not found for user {UserId}", model.MenuItemId, User.Identity.Name);
                    return Json(new { success = false, message = "Menu item not found." });
                }

                if (!menuItem.IsAvailable)
                {
                    _logger.LogWarning("Attempted to add unavailable item {MenuItemId} for user {UserId}", model.MenuItemId, User.Identity.Name);
                    return Json(new { success = false, message = $"{menuItem.Name} is currently unavailable." });
                }

                // Validate customizations
                if (model.Customizations?.Any() == true)
                {
                    var validOptions = await _menuItemRepository.GetCustomizationOptionsAsync(model.MenuItemId);
                    var validOptionIds = validOptions.Select(o => o.Id).ToHashSet();
                    var validChoices = validOptions
                        .SelectMany(o => o.Choices)
                        .ToDictionary(c => c.Id, c => c.Price);

                    foreach (var customization in model.Customizations)
                    {
                        if (!validOptionIds.Contains(customization.OptionId) || !validChoices.ContainsKey(customization.ChoiceId))
                        {
                            _logger.LogWarning("Invalid customization for item {MenuItemId}: OptionId={OptionId}, ChoiceId={ChoiceId}, User={UserId}",
                                model.MenuItemId, customization.OptionId, customization.ChoiceId, User.Identity.Name);
                            return Json(new { success = false, message = "Invalid customization selected." });
                        }
                        customization.Price = validChoices[customization.ChoiceId];
                    }
                }

                var cartItem = new CartItemViewModel
                {
                    MenuItemId = model.MenuItemId,
                    Quantity = model.Quantity,
                    Name = menuItem.Name,
                    ImageUrl = menuItem.ImageUrl,
                    Price = menuItem.Price,
                    RestaurantId = menuItem.RestaurantId,
                    RestaurantName = menuItem.Restaurant.Name,
                    Customizations = model.Customizations ?? new List<CustomizationViewModel>()
                };

                await _cartService.AddItemToCartAsync(User.Identity.Name, cartItem);
                _logger.LogInformation("Added {Quantity} of {MenuItemName} to cart for user {UserId}", model.Quantity, menuItem.Name, User.Identity.Name);

                var cartCount = await _cartService.GetCartItemCountAsync(User.Identity.Name);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = true, message = $"{model.Quantity} {menuItem.Name} added to cart.", cartCount });
                }

                TempData["SuccessMessage"] = $"{menuItem.Name} added to cart";
                return RedirectToAction("Details", "Restaurants", new { id = menuItem.RestaurantId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item {MenuItemId} to cart for user {UserId}", model.MenuItemId, User.Identity.Name);
                if (Request.Headers["X-Requested-With"] == "XMLHttpRequest")
                {
                    return Json(new { success = false, message = "An error occurred while adding to cart." });
                }

                TempData["ErrorMessage"] = "Failed to add item to cart.";
                return RedirectToAction("Details", "Restaurants", new { id = model.RestaurantId });
            }
        }

        // POST: Cart/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, int quantity)
        {
            if (cartItemId <= 0 || quantity <= 0 || quantity > 100)
            {
                _logger.LogWarning("Invalid update data: CartItemId={CartItemId}, Quantity={Quantity}, User={UserId}", cartItemId, quantity, User.Identity.Name);
                TempData["ErrorMessage"] = "Invalid quantity or item data.";
                return RedirectToAction("Index");
            }

            try
            {
                await _cartService.UpdateItemQuantityAsync(User.Identity.Name, cartItemId, quantity);
                _logger.LogInformation("Updated cart item {CartItemId} to quantity {Quantity} for user {UserId}", cartItemId, quantity, User.Identity.Name);
                TempData["SuccessMessage"] = "Cart item quantity updated.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item {CartItemId} for user {UserId}", cartItemId, User.Identity.Name);
                TempData["ErrorMessage"] = "Failed to update cart item.";
                return RedirectToAction("Index");
            }
        }

        // POST: Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            if (cartItemId <= 0)
            {
                _logger.LogWarning("Invalid remove data: CartItemId={CartItemId}, User={UserId}", cartItemId, User.Identity.Name);
                TempData["ErrorMessage"] = "Invalid item data.";
                return RedirectToAction("Index");
            }

            try
            {
                await _cartService.RemoveItemFromCartAsync(User.Identity.Name, cartItemId);
                _logger.LogInformation("Removed cart item {CartItemId} from cart for user {UserId}", cartItemId, User.Identity.Name);
                TempData["SuccessMessage"] = "Item removed from cart.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item {CartItemId} for user {UserId}", cartItemId, User.Identity.Name);
                TempData["ErrorMessage"] = "Failed to remove item from cart.";
                return RedirectToAction("Index");
            }
        }

        // POST: Cart/Clear
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                await _cartService.ClearCartAsync(User.Identity.Name);
                _logger.LogInformation("Cleared cart for user {UserId}", User.Identity.Name);
                TempData["SuccessMessage"] = "Cart cleared successfully.";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user {UserId}", User.Identity.Name);
                TempData["ErrorMessage"] = "Failed to clear cart.";
                return RedirectToAction("Index");
            }
        }

        // GET: Cart/Summary
        [HttpGet]
        public async Task<IActionResult> CartSummary()
        {
            try
            {
                var count = await _cartService.GetCartItemCountAsync(User.Identity.Name);
                return PartialView("_CartSummary", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart summary for user {UserId}", User.Identity.Name);
                return PartialView("_CartSummary", 0);
            }
        }

        // calculate delivery fee from uniqe restaurants 
        private decimal CalculateDeliveryFee(List<CartItemViewModel> cartItems)
        {
            var uniqueRestaurants = cartItems.Sum(
                r => r.DeliveryFee * r.Quantity / cartItems.Count(i => i.RestaurantId == r.RestaurantId)
                );
            return uniqueRestaurants;
        }


        // calculate Tax fee 
        private decimal CalculateTaxFee(List<CartItemViewModel> cartItems)
        {
            var taxRate = cartItems.FirstOrDefault()?.TaxRate ?? 0;
            var subtotal = cartItems.Sum(item => (item.Price + item.Customizations.Sum(c => c.Price)) * item.Quantity);
            return subtotal * taxRate / 100;
        }
    }
}