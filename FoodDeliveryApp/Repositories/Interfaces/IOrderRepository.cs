using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderWithDetailsAsync(int orderId);
        //GetRecentOrdersAsync
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count);
    }
}
