using FoodDeliveryApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        IQueryable<Order> GetUserOrders(string userId, params Expression<Func<Order, object>>[] includes);
        IQueryable<Order> GetAllOrders(params Expression<Func<Order, object>>[] includes);
        Task<Order?> GetOrderWithDetailsAsync(int orderId);
        IQueryable<Order> GetRecentOrders(int count, params Expression<Func<Order, object>>[] includes);
        Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status);
        Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus);
        // cancel order
        Task<Order?> CancelOrderAsync(int orderId);
        Task<IEnumerable<Order>> GetOrdersByRestaurantAsync(int restaurantId);

        Task<Order> SetDeliveryTimeAsync(int orderId, DateTime deliveryTime);
    }
}
