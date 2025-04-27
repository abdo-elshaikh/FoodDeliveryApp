using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        private readonly ApplicationDbContext _context;
        public PromotionRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public async Task<Promotion> GetByCodeAsync(string code)
            => await _context.Promotions
                .FirstOrDefaultAsync(p => p.Code == code);

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
            => await _context.Promotions
                .Where(p => p.IsActive &&
                           p.StartDate <= DateTime.UtcNow &&
                           p.EndDate >= DateTime.UtcNow)
                .ToListAsync();

        public async Task<IEnumerable<Promotion>> GetExpiredPromotionsAsync()
            => await _context.Promotions
                .Where(p => p.EndDate < DateTime.UtcNow)
                .ToListAsync();

        public async Task<bool> IsPromotionValidAsync(string code, decimal orderAmount)
        {
            var promotion = await GetByCodeAsync(code);
            if (promotion == null) return false;

            return promotion.IsActive &&
                   promotion.StartDate <= DateTime.UtcNow &&
                   promotion.EndDate >= DateTime.UtcNow &&
                   (promotion.MinimumOrderAmount == null ||
                    orderAmount >= promotion.MinimumOrderAmount) &&
                   (promotion.UsageLimit == null ||
                    await GetUsageCountAsync(code) < promotion.UsageLimit);
        }

        private async Task<int> GetUsageCountAsync(string code)
            => await _context.Orders
                .CountAsync(o => o.Equals(code));

        // GetValidPromotion
        public async Task<Promotion> GetValidPromotionAsync(string code, int restaurantId, decimal orderAmount)
        {
            return await _context.Promotions
                .FirstOrDefaultAsync(p => p.Code == code &&
            p.RestaurantId == restaurantId &&
            p.IsActive &&
            p.StartDate <= DateTime.UtcNow &&
            p.EndDate >= DateTime.UtcNow &&
            (p.MinimumOrderAmount == 0 || orderAmount >= p.MinimumOrderAmount));

        }
    }
}