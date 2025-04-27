using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IPaymentRepository : IRepository<Payment>
    {
        Task<IEnumerable<Payment>> GetPaymentsByUserAsync(string userId);
        Task<IEnumerable<Payment>> GetPaymentsByOrderAsync(int orderId);
        Task<decimal> GetTotalRevenueByPeriodAsync(DateTime startDate, DateTime endDate);
        Task<int> GetSuccessfulPaymentCountAsync(int restaurantId);
    }
}