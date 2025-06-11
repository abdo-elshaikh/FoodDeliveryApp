using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using FoodDeliveryApp.Services.Interfaces;
using System.Text.Json;

namespace FoodDeliveryApp.Services
{
    public class CartService : ICartService
    {

        private readonly IUnitOfWork _unitOfWork;
        private readonly ICurrentUserService _currentUserService;

        public CartService(IUnitOfWork unitOfWork, ICurrentUserService currentUserService)
        {
            _unitOfWork = unitOfWork;
            _currentUserService = currentUserService;
        }
        public async Task<Cart> AddItemAsync(string userId, int menuItemId, int quantity, string specialInstructions = null, List<MenuItem> modifiers = null)
        {
            var cart = await GetCartAsync(userId);

            var menuItem = await _unitOfWork.MenuItems.GetByIdAsync(menuItemId);
            if (menuItem == null)
                throw new ArgumentException("Menu item not found");

            var cartItem = cart.Items.FirstOrDefault(i =>
                i.MenuItemId == menuItemId &&
                i.SpecialInstructions == specialInstructions);

            if (cartItem != null)
            {
                cartItem.Quantity += quantity;
            }
            else
            {
                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    MenuItemId = menuItemId,
                    Quantity = quantity,
                    SpecialInstructions = specialInstructions,
                    RestaurantId = menuItem.RestaurantId
                };
                cart.Items.Add(cartItem);
            }

            cart.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> ApplyPromotionAsync(string userId, string promotionCode)
        {
            var cart = await GetCartAsync(userId);

            var promotion = await _unitOfWork.Promotions.GetByCodeAsync(promotionCode);
            if (promotion == null || promotion.ValidUntil < DateTime.UtcNow)
                throw new ArgumentException("Invalid or expired promotion code");

            decimal discountAmount = promotion.CalculateDiscountAmount(cart.Subtotal);

            cart.ApplyPromotion(promotionCode, discountAmount, promotion.ValidUntil);

            cart.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async void ClearCart()
        {
            // This method is not part of the interface and is async void, which is not recommended.
            // Implementing as a synchronous wrapper for ClearCartAsync for compatibility.
            throw new NotImplementedException("Use ClearCartAsync instead.");
        }

        public async Task<Cart> ClearCartAsync(string userId)
        {
            var cart = await GetCartAsync(userId);

            cart.Clear();

            cart.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async Task<Order> ConvertCartToOrderAsync(string userId, int deliveryAddressId, PaymentMethod paymentMethod)
        {
            var cart = await GetCartAsync(userId);

            if (!cart.CanCheckout)
                throw new InvalidOperationException("Cart cannot be checked out");

            var order = new Order
            {
                UserId = userId,
                DeliveryAddressId = deliveryAddressId,
                Status = OrderStatus.Pending,
                OrderDate = DateTime.UtcNow,
                Subtotal = cart.Subtotal,
                Tax = cart.Tax,
                DeliveryFee = cart.DeliveryFee,
                Discount = cart.PromotionDiscountAmount,
                Total = cart.Total,
                PaymentMethod = paymentMethod,
                PaymentStatus = PaymentStatus.Pending,
                Notes = cart.SpecialInstructions
            };

            await _unitOfWork.Orders.AddAsync(order);
            await _unitOfWork.SaveChangesAsync();

            foreach (var item in cart.Items)
            {
                var orderItem = new OrderItem
                {
                    OrderId = order.Id,
                    MenuItemId = item.MenuItemId,
                    RestaurantId = item.RestaurantId,
                    Quantity = item.Quantity,
                    Price = item.MenuItem.Price,
                    SpecialInstructions = item.SpecialInstructions,
                };
                await _unitOfWork.OrderItems.AddAsync(orderItem);
            }

            cart.Status = CartStatus.CheckedOut;
            cart.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return order;
        }

        public async Task<Cart> ExtendCartExpirationAsync(string userId, TimeSpan duration)
        {
            var cart = await GetCartAsync(userId);

            cart.ExtendExpiration(duration);

            cart.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> GetCartAsync(string userId)
        {
            var cart = await _unitOfWork.Carts.GetByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart
                {
                    UserId = userId,
                    Status = CartStatus.Active,
                    CreatedAt = DateTime.UtcNow,
                    LastModifiedAt = DateTime.UtcNow,
                    ExpiresAt = DateTime.UtcNow.AddHours(24)
                };
                await _unitOfWork.Carts.AddAsync(cart);
                await _unitOfWork.SaveChangesAsync();
            }
            return cart;
        }

        public async Task<List<CartItem>> GetCartItemsAsync(string userId)
        {
            var cart = await GetCartAsync(userId);
            var items = await _unitOfWork.CartItems.GetByCartIdAsync(cart.Id);
            return items;
        }

        public async Task<Cart> RemoveItemAsync(string userId, int itemId)
        {
            var cart = await GetCartAsync(userId);

            var cartItem = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (cartItem == null)
                throw new ArgumentException("Cart item not found");

            cart.Items.Remove(cartItem);

            cart.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> RemovePromotionAsync(string userId)
        {
            var cart = await GetCartAsync(userId);

            cart.RemovePromotion();

            cart.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }

        public async Task<Cart> UpdateItemQuantityAsync(string userId, int itemId, int quantity)
        {
            var cart = await GetCartAsync(userId);

            var cartItem = cart.Items.FirstOrDefault(i => i.Id == itemId);
            if (cartItem == null)
                throw new ArgumentException("Cart item not found");

            if (quantity <= 0)
            {
                cart.Items.Remove(cartItem);
            }
            else
            {
                cartItem.Quantity = quantity;
            }

            cart.LastModifiedAt = DateTime.UtcNow;

            await _unitOfWork.SaveChangesAsync();

            return cart;
        }
    }
}