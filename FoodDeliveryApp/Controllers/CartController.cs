using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IMenuItemRepository _menuItemRepository;
        private readonly ILogger<CartController> _logger;
        private readonly UserManager<ApplicationUser> _userManager;

        public CartController(
            ICartService cartService,
            IMenuItemRepository menuItemRepository,
            UserManager<ApplicationUser> userManager,
            ILogger<CartController> logger)
        {
            _cartService = cartService;
            _menuItemRepository = menuItemRepository;
            _userManager = userManager;
            _logger = logger;
        }

        // GET: Cart/Index
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                var cart = await _cartService.GetCartAsync(user.Id);
                var cartItems = cart.Items.Select(item => new CartItemViewModel
                {
                    Id = item.Id,
                    MenuItemId = item.MenuItemId,
                    Quantity = item.Quantity,
                    Name = item.MenuItem?.Name ?? "Unnamed Item",
                    ImageUrl = item.MenuItem?.ImageUrl ?? "/images/placeholder-dish.jpg",
                    Price = item.MenuItem?.Price ?? 0,
                    RestaurantId = item.MenuItem?.RestaurantId ?? 0,
                    RestaurantName = item.MenuItem?.Restaurant?.Name ?? "Unknown Restaurant",
                    TaxRate = item.MenuItem?.Restaurant?.TaxRate ?? 0,
                    DeliveryFee = item.MenuItem?.Restaurant?.DeliveryFee ?? 0,
                    Customizations = item.Customizations?.Select(c => new CartItemCustomizationViewModel
                    {
                        OptionId = c.OptionId,
                        ChoiceId = c.ChoiceId,
                        Price = c.Price
                    }).ToList() ?? new List<CartItemCustomizationViewModel>()
                }).ToList();

                var viewModel = new CartViewModel
                {
                    Items = cartItems,
                    Subtotal = cartItems.Sum(item => (item.Price + (item.Customizations?.Sum(c => c.Price) ?? 0)) * item.Quantity),
                    DeliveryFee = CalculateDeliveryFee(cartItems),
                    Tax = CalculateTaxFee(cartItems),
                    Total = cartItems.Sum(item => (item.Price + (item.Customizations?.Sum(c => c.Price) ?? 0)) * item.Quantity) +
                           CalculateDeliveryFee(cartItems) + CalculateTaxFee(cartItems)
                };

                return View(viewModel);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart for user ID {UserId}", User.Identity?.Name);
                TempData["ErrorMessage"] = "Unable to load your cart. Please try again later.";
                return RedirectToAction("Index", "Home");
            }
        }

        // POST: Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add")]
        public async Task<IActionResult> AddToCart(AddToCartViewModel model)
        {
            if (model == null || model.MenuItemId <= 0 || model.Quantity <= 0 || model.Quantity > 100)
            {
                _logger.LogWarning("Invalid add to cart data: {ModelData}, User={UserId}",
                    model != null ? $"MenuItemId={model.MenuItemId}, Quantity={model.Quantity}" : "null model",
                    User.Identity?.Name);
                TempData["ErrorMessage"] = "Invalid item data or quantity.";
                return RedirectToAction("Index");
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                var menuItem = await _menuItemRepository.GetByIdAsync(model.MenuItemId);

                if (menuItem == null || !menuItem.IsAvailable)
                {
                    _logger.LogWarning("Attempted to add unavailable item to cart: MenuItemId={MenuItemId}, User={UserId}",
                        model.MenuItemId, user.Id);
                    TempData["ErrorMessage"] = "This item is no longer available.";
                    return RedirectToAction("Details", "MenuItems", new { id = model.MenuItemId });
                }

               var castomizations = model.Customizations
                    .Select(c => new CartItemCustomizationViewModel
                    {
                        OptionId = c.OptionId,
                        ChoiceId = c.ChoiceId,
                        Price = c.Price
                    }).ToList();

                var cartItem = new CartItemViewModel
                {
                    MenuItemId = model.MenuItemId,
                    Quantity = model.Quantity,
                    Name = menuItem.Name,
                    ImageUrl = menuItem.ImageUrl,
                    RestaurantId = menuItem.RestaurantId,
                    Customizations = castomizations
                };

                // Add item to cart
                await _cartService.AddItemToCartAsync(user.Id, cartItem);
                _logger.LogInformation("Added item {MenuItemId} to cart with quantity {Quantity} for user {UserId}",
                    model.MenuItemId, model.Quantity, user.Id);

                TempData["SuccessMessage"] = $"{menuItem.Name} added to your cart.";
                ViewBag.CartCount = await _cartService.GetCartItemCountAsync(user.Id);
                return RedirectToAction("Details", "MenuItems", new { id = model.MenuItemId });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item {MenuItemId} to cart for user {UserId}",
                    model.MenuItemId, User.Identity?.Name);
                TempData["ErrorMessage"] = "Failed to add item to cart. Please try again.";
                return RedirectToAction("Details", "MenuItems", new { id = model.MenuItemId });
            }
        }

        // POST: Cart/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update")]
        public async Task<IActionResult> UpdateCartItem(int cartItemId, int quantity)
        {
            if (cartItemId <= 0 || quantity <= 0 || quantity > 100)
            {
                _logger.LogWarning("Invalid update data: CartItemId={CartItemId}, Quantity={Quantity}, User={UserId}", cartItemId, quantity, User.Identity?.Name);
                TempData["ErrorMessage"] = "Invalid quantity or item data.";
                return RedirectToAction("Index");
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _cartService.UpdateItemQuantityAsync(user.Id, cartItemId, quantity);
                _logger.LogInformation("Updated cart item {CartItemId} to quantity {Quantity} for user {UserId}", cartItemId, quantity, user.Id);
                TempData["SuccessMessage"] = "Cart item quantity updated.";
                ViewBag.CartCount = await _cartService.GetCartItemCountAsync(user.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating cart item {CartItemId} for user {UserId}", cartItemId, User.Identity?.Name);
                TempData["ErrorMessage"] = "Failed to update cart item.";
                return RedirectToAction("Index");
            }
        }

        // POST: Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("remove")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            if (cartItemId <= 0)
            {
                _logger.LogWarning("Invalid remove data: CartItemId={CartItemId}, User={UserId}", cartItemId, User.Identity?.Name);
                TempData["ErrorMessage"] = "Invalid item data.";
                return RedirectToAction("Index");
            }

            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _cartService.RemoveItemFromCartAsync(user.Id, cartItemId);
                _logger.LogInformation("Removed cart item {CartItemId} from cart for user {UserId}", cartItemId, user.Id);
                TempData["SuccessMessage"] = "Item removed from cart.";
                ViewBag.CartCount = await _cartService.GetCartItemCountAsync(user.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item {CartItemId} for user {UserId}", cartItemId, User.Identity?.Name);
                TempData["ErrorMessage"] = "Failed to remove item from cart.";
                return RedirectToAction("Index");
            }
        }

        // POST: Cart/Clear
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("clear")]
        public async Task<IActionResult> ClearCart()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                await _cartService.ClearCartAsync(user.Id);
                _logger.LogInformation("Cleared cart for user {UserId}", user.Id);
                TempData["SuccessMessage"] = "Cart cleared successfully.";
                ViewBag.CartCount = await _cartService.GetCartItemCountAsync(user.Id);
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart for user {UserId}", User.Identity?.Name);
                TempData["ErrorMessage"] = "Failed to clear cart.";
                return RedirectToAction("Index");
            }
        }

        // GET: Cart/Summary
        [HttpGet]
        [Route("summary")]
        [AllowAnonymous]
        public async Task<IActionResult> CartSummary()
        {
            try
            {
                var user = await _userManager.GetUserAsync(User);
                if(user == null) return PartialView("_CartSummary", 0);
                var count = await _cartService.GetCartItemCountAsync(user.Id);
                return PartialView("_CartSummary", count);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart summary for user {UserId}", User.Identity?.Name);
                return PartialView("_CartSummary", 0);
            }
        }

        // Calculate delivery fee from unique restaurants
        private decimal CalculateDeliveryFee(List<CartItemViewModel> cartItems)
        {
            if (!cartItems.Any())
            {
                return 0;
            }

            var uniqueRestaurantIds = cartItems.Select(item => item.RestaurantId).Distinct();
            return uniqueRestaurantIds.Sum(id => cartItems.First(item => item.RestaurantId == id).DeliveryFee);
        }

        // Calculate tax fee
        private decimal CalculateTaxFee(List<CartItemViewModel> cartItems)
        {
            if (!cartItems.Any())
            {
                return 0;
            }

            // Assume tax rate is consistent within a restaurant; use the first item's tax rate
            var taxRate = cartItems.FirstOrDefault()?.TaxRate ?? 0;
            var subtotal = cartItems.Sum(item => (item.Price + (item.Customizations?.Sum(c => c.Price) ?? 0)) * item.Quantity);
            return subtotal * taxRate / 100;
        }
    }
}