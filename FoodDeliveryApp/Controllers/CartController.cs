using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services;
using FoodDeliveryApp.Services.Interfaces;
using FoodDeliveryApp.ViewModels.Cart;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using System.IO;

namespace FoodDeliveryApp.Controllers
{
    [Authorize]
    [Route("cart")]
    public class CartController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<CartController> _logger;
        private readonly ICurrentUserService _userManager;

        public CartController(IUnitOfWork unitOfWork, ICurrentUserService userManager, ILogger<CartController> logger)
        {
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }


        // GET: Cart/Index
        [HttpGet]
        [Route("")]
        public async Task<IActionResult> Index()
        {
            var userId = _userManager.GetCurrentUserId();
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);

            if (cart == null)
            {
                _logger.LogWarning("Cart not found for user {UserId}", userId);
                return NotFound("Cart not found.");
            }

            if (cart.IsEmpty)
            {
                return View(new CartViewModel(new List<CartItemViewModel>(), 0, 0, 0));
            }

            var deliveryFee = CalculateDeliveryFee(cart);
            var tax = CalculateTaxFee(cart);
            var total = CalculateTotal(cart, deliveryFee, tax);

            var cartViewModel = new CartViewModel(
                cart.Items.Select(x => new CartItemViewModel(x)).ToList(),
                total,
                deliveryFee,
                tax);

            return View(cartViewModel);
        }

        // POST: Cart/Add
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("add")]
        public async Task<IActionResult> AddToCart(int menuItemId, int quantity = 1)
        {
            try
            {
                // Validate input
                if (quantity < 1 || quantity > 99)
                {
                    _logger.LogWarning("Invalid quantity {Quantity} for menu item {MenuItemId}", quantity, menuItemId);
                    return BadRequest(new { success = false, message = "Invalid quantity. Must be between 1 and 99." });
                }

                var userId = _userManager.GetCurrentUserId();
                if (userId == null) return Challenge();

                var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(menuItemId);
                if (menuItem == null)
                {
                    _logger.LogWarning("Menu item not found. ID: {MenuItemId}", menuItemId);
                    return NotFound(new { success = false, message = "Menu item not found." });
                }

                // Check if item is available
                if (!menuItem.IsAvailable)
                {
                    _logger.LogWarning("Menu item {MenuItemId} is not available", menuItemId);
                    return BadRequest(new { success = false, message = "This item is currently unavailable." });
                }

                var cart = await _unitOfWork.Carts.AddItemToCartAsync(userId, menuItem, quantity);

                if (cart == null)
                {
                    _logger.LogWarning("Error adding item to cart for user {UserId}", userId);
                    return BadRequest(new { success = false, message = "Error adding item to cart." });
                }

                _logger.LogInformation("Item added to cart for user {UserId}", userId);

                // Get cart summary data
                var cartSummary = await CartSummary();
                var cartSummaryViewModel = cartSummary as CartSummaryViewModel;

                return Ok(new
                {
                    success = true,
                    message = "Item added to cart successfully",
                    cartItemCount = cart.Items.Count,
                    restaurantId = menuItem.RestaurantId,
                    cartSummaryViewModel = cartSummaryViewModel
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding item to cart for user {UserId}", _userManager.GetCurrentUserId());
                return StatusCode(500, new { success = false, message = "An error occurred while adding item to cart." });
            }
        }

        // POST: Cart/Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("update")]
        public async Task<IActionResult> UpdateQuantity(int cartItemId, int quantity)
        {
            try
            {

                var userId = _userManager.GetCurrentUserId();
                if (userId == null) return Challenge();

                var cart = await _unitOfWork.Carts.UpdateCartItemQuantityAsync(userId, cartItemId, quantity);

                _logger.LogInformation("Quantity for cart item {CartItemId} updated for user {UserId}", cartItemId, userId);
                TempData["Success"] = "Item quantity updated successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating quantity for cart item {CartItemId}", cartItemId);
                TempData["Error"] = "An error occurred while updating the item quantity.";
                return RedirectToAction(nameof(Index));
            }
        }

        // POST: Cart/Remove
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("remove")]
        public async Task<IActionResult> RemoveFromCart(int cartItemId)
        {
            try
            {
                var userId = _userManager.GetCurrentUserId();
                if (userId == null) return Challenge();

                var cart = await _unitOfWork.Carts.RemoveItemFromCartAsync(userId, cartItemId);

                _logger.LogInformation("Cart item {CartItemId} removed from cart for user {UserId}",
                    cartItemId, userId);
                TempData["Success"] = "Item removed from cart successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error removing cart item {CartItemId}", cartItemId);
                TempData["Error"] = "An error occurred while removing the item from your cart.";
                return RedirectToAction(nameof(Index));
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
                var userId = _userManager.GetCurrentUserId();
                if (userId == null) return Challenge();

                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart == null)
                {
                    _logger.LogWarning("Cart not found for user {UserId}", userId);
                    return NotFound("Cart not found.");
                }

                await _unitOfWork.Carts.ClearCartItemsAsync(cart.Id);

                _logger.LogInformation("Cart cleared for user {UserId}", userId);
                TempData["Success"] = "Cart cleared successfully.";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error clearing cart");
                TempData["Error"] = "An error occurred while clearing your cart.";
                return RedirectToAction(nameof(Index));
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
                var userId = _userManager.GetCurrentUserId();
                if (userId == null) return PartialView("_CartSummary", 0);
                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);

                if (cart == null) return PartialView("_CartSummary", 0);

                var deliveryFee = CalculateDeliveryFee(cart);
                var tax = CalculateTaxFee(cart);
                var total = CalculateTotal(cart, deliveryFee, tax);

                var ItemCount = cart.Items.Count();
                var Total = CalculateTotal(cart, deliveryFee, tax);
                return PartialView("_CartSummary", new CartSummaryViewModel { ItemCount = ItemCount, Total = Total });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error loading cart summary for user {UserId}", User.Identity?.Name);
                return PartialView("_CartSummary", 0);
            }
        }

        // POST: Cart/ApplyPromoCode
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Cart/ApplyPromoCode/{promoCode}")]
        public async Task<IActionResult> ApplyPromoCode(string promoCode)
        {
            var userId = _userManager.GetCurrentUserId();
            if (userId == null) return BadRequest("Invalid user.");
            try
            {
                var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
                if (cart == null) return BadRequest("Cart not found.");
                var promo = await _unitOfWork.Promotions.GetByCodeAsync(promoCode);
                if (promo == null)
                {
                    _logger.LogWarning("Invalid promotion code {PromoCode}", promoCode);
                    return BadRequest("Invalid promotion code.");
                }
                var discountAmount = promo.CalculateDiscountAmount(cart.Subtotal);
                cart.PromotionCode = promoCode;
                cart.IsPromotionApplied = true;
                cart.PromotionCodeExpiration = promo.ValidUntil;
                cart.LastModifiedAt = DateTime.UtcNow;
                cart.TotalWithDiscount = cart.Items.Sum(x => x.Subtotal) - discountAmount;

                await _unitOfWork.Carts.UpdateAsync(cart);
                _logger.LogInformation("Promotion code {PromoCode} applied to cart for user {UserId}", promoCode, userId);
                return Json(new { success = true, discountAmount = discountAmount });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying promotion code {PromoCode} to cart for user {UserId}", promoCode, userId);
                return BadRequest("An error occurred while applying the promotion code.");
            }

        }

        // Calculate delivery fee from unique restaurants
        private decimal CalculateDeliveryFee(Cart cart)
        {
            var uniqueRestaurants = cart.Items.Select(item => item.RestaurantId).Distinct().ToList();
            var deliveryFee = 0m;
            foreach (var restaurant in uniqueRestaurants)
            {
                deliveryFee += cart.Items.Where(item => item.RestaurantId == restaurant).Sum(item => item.Restaurant.DeliveryFee);
            }
            return deliveryFee;
        }

        // Calculate tax fee
        private decimal CalculateTaxFee(Cart cart)
        {
            var taxFee = 0m;
            foreach (var item in cart.Items)
            {
                taxFee += item.MenuItem.Price * item.MenuItem.Restaurant.TaxRate / 100;
            }
            return taxFee;
        }

        // Calculate total
        private decimal CalculateTotal(Cart cart, decimal deliveryFee, decimal tax)
        {
            var itemsTotal = cart.Items.Sum(item => item.MenuItem.Price * item.Quantity);
            return itemsTotal + deliveryFee + tax;
        }


    }
}
