using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface ICartRepository : IRepository<Cart>
    {
        // create user cart
        Task<Cart> CreateUserCartAsync(string userId);
        // get cart by userId
        Task<Cart> GetByUserIdAsync(string userId);
        // add item to cart
        Task<Cart> AddItemToCartAsync(string userId, MenuItem menuItem, int quantity);

        // remove item from cart
        Task<Cart> RemoveItemFromCartAsync(string userId, int cartItemId);

        // update cart item quantity
        Task<Cart> UpdateCartItemQuantityAsync(string userId, int cartItemId, int quantity);

        // clear cart items
        Task ClearCartItemsAsync(int cartId);
    }
}
