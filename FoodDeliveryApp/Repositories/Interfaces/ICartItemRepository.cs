using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface ICartItemRepository : IRepository<CartItem>
    {
        // get by cartId
        Task<List<CartItem>> GetByCartIdAsync(int cartId);
        // add to cart
        Task AddToCartAsync(CartItem cartItem);
    }
}
