using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Order;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface IOrderService
    {
        Task<PaginatedList<Order>> SearchOrdersAsync(
            string searchTerm = null,
            string statusFilter = null,
            DateTime? startDate = null,
            DateTime? endDate = null,
            int page = 1,
            int pageSize = 10,
            string sortBy = "OrderDate",
            string sortOrder = "desc");

        Task<Order> GetOrderByIdAsync(int id);
        Task<Order> CreateOrderAsync(OrderCreateViewModel model);
        Task<Order> UpdateOrderStatusAsync(int id, OrderStatus status);
        Task<Order> CancelOrderAsync(int id);

        Task<IEnumerable<Order>> GetUserOrdersAsync(string userId);
        Task<Order> AssignDriverAsync(int orderId, int driverId);
        Task<Order> UpdateOrderTrackingAsync(int orderId, string location, string status);
        Task<decimal> CalculateOrderTotalAsync(OrderCreateViewModel model);
        Task<IEnumerable<OrderTrackingViewModel>> GetOrderTrackingHistoryAsync(int orderId);

        //SetDeliveryTime
        Task<Order> SetDeliveryTimeAsync(int orderId, DateTime deliveryTime);

        Task<Order> UpdateOrderAsync(Order order);

        // New methods
        Task<bool> IsRestaurantOwnerAsync(int restaurantId, string userId);
        Task<Order> UpdateOrderItemStatusAsync(int orderId, int itemId, OrderItemStatus status);
        Task<byte[]> ExportOrdersAsync(
            string searchTerm = null,
            string statusFilter = null,
            DateTime? startDate = null,
            DateTime? endDate = null);
        Task<byte[]> GenerateOrderPdfAsync(OrderDetailsViewModel order);
    }
}
