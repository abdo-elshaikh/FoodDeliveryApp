using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Cart;

namespace FoodDeliveryApp.Services
{
    public interface ICartService
    {
        Task<int> GetCartItemCountAsync(string userId);
        Task<Cart> GetCartAsync(string userId);
        Task AddItemToCartAsync(string userId, CartItemViewModel cartItem);
        Task UpdateItemQuantityAsync(string userId, int cartItemId, int quantity);
        Task RemoveItemFromCartAsync(string userId, int cartItemId);
        Task ClearCartAsync(string userId);
        Task<decimal> GetCartTotalAsync(string userId);
    }
}