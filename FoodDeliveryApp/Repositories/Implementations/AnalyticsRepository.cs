using FoodDeliveryApp.Data;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace FoodDeliveryApp.Repositories.Implementations
{
    public class AnalyticsRepository : IAnalyticsRepository
    {
        private readonly ApplicationDbContext _context;

        public AnalyticsRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Dictionary<string, decimal>> GetSalesByCategoryAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.Orders
                .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                .SelectMany(o => o.OrderItems)
                .GroupBy(oi => oi.MenuItem.Restaurant.Name)
                .Select(g => new { Category = g.Key, TotalSales = g.Sum(oi => oi.Price) })
                .ToDictionaryAsync(x => x.Category, x => x.TotalSales);
        }

        public async Task<Dictionary<int, int>> GetPopularMenuItemsAsync(int count)
        {
            return await _context.OrderItems
                .GroupBy(oi => oi.MenuItemId)
                .OrderByDescending(g => g.Count())
                .Take(count)
                .Select(g => new { MenuItemId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.MenuItemId, x => x.Count);
        }

        public async Task<Dictionary<TimeSpan, int>> GetOrderDistributionByTimeAsync()
        {
            return await _context.Orders
                .GroupBy(o => o.OrderDate.TimeOfDay)
                .Select(g => new { Time = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Time, x => x.Count);
        }

        public async Task<Dictionary<string, int>> GetCustomerOrderFrequencyAsync()
        {
            return await _context.Orders
                .GroupBy(o => o.UserId)
                .Select(g => new { UserId = g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.UserId, x => x.Count);
        }
    }
}