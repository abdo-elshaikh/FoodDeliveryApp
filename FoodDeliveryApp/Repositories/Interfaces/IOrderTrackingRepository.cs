using FoodDeliveryApp.Models;


namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IOrderTrackingRepository : IRepository<OrderTracking>
    {
        Task<IEnumerable<OrderTracking>> GetOrderTrackingHistoryAsync(int orderId);
        Task<OrderTracking> GetLatestTrackingAsync(int orderId);
        Task<OrderTracking> AddTrackingUpdateAsync(OrderTracking tracking);
        Task<IEnumerable<OrderTracking>> GetDriverActiveDeliveriesAsync(int driverId);
        Task<IEnumerable<OrderTracking>> GetPendingDeliveriesAsync();
        Task<OrderTracking> CreatedAtDeliveryAsync(int orderId, int deliveryId, int addressId);
        Task<bool> UpdateDriverLocationAsync(int orderId, int driverId, double latitude, double longitude, string? address);
    }
}
