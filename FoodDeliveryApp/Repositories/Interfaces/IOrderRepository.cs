using FoodDeliveryApp.Models;
using System.Linq.Expressions;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId, params Expression<Func<Order, object>>[] includes);
        Task<IEnumerable<Order>> GetAllOrdersAsync(params Expression<Func<Order, object>>[] includes);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        //GetRecentOrdersAsync
        Task<IEnumerable<Order>> GetRecentOrdersAsync(int count, params Expression<Func<Order, object>>[] includes);
    }
}
