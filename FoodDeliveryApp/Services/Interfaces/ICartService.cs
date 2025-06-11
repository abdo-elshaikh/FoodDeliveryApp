using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface ICartService
    {
        Task<Cart> GetCartAsync(string userId);
        Task<Cart> AddItemAsync(string userId, int menuItemId, int quantity, string specialInstructions = null, List<MenuItem> modifiers = null);
        Task<Cart> UpdateItemQuantityAsync(string userId, int itemId, int quantity);
        Task<Cart> RemoveItemAsync(string userId, int itemId);
        Task<Cart> ClearCartAsync(string userId);
        Task<Cart> ApplyPromotionAsync(string userId, string promotionCode);
        Task<Cart> RemovePromotionAsync(string userId);
        Task<Cart> ExtendCartExpirationAsync(string userId, TimeSpan duration);
        Task<Order> ConvertCartToOrderAsync(string userId, int deliveryAddressId, PaymentMethod paymentMethod);

        // Get Cart Items
        Task<List<CartItem>> GetCartItemsAsync(string userId);
    }
} 