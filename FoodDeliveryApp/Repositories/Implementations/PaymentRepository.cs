using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class PaymentRepository : Repository<Payment>, IPaymentRepository
    {
        private readonly ApplicationDbContext _context;
        public PaymentRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Payment>> GetPaymentsByUserAsync(string userId)
            => await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.Order.UserId == userId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

        public async Task<IEnumerable<Payment>> GetPaymentsByOrderAsync(int orderId)
            => await _context.Payments
                .Where(p => p.OrderId == orderId)
                .OrderByDescending(p => p.PaymentDate)
                .ToListAsync();

        public async Task<decimal> GetTotalRevenueByPeriodAsync(DateTime startDate, DateTime endDate)
            => await _context.Payments
                .Where(p => p.PaymentDate >= startDate && p.PaymentDate <= endDate && p.Status == PaymentStatus.Completed)
                .SumAsync(p => p.Amount);

        public async Task<int> GetSuccessfulPaymentCountAsync(int restaurantId)
        {
            return await _context.Payments
                .Include(p => p.Order)
                .Where(p => p.Order.RestaurantId == restaurantId && p.Status == PaymentStatus.Completed)
                .CountAsync();
        }
    }
}