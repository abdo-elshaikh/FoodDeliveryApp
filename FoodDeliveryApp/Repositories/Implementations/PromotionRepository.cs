using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class PromotionRepository : Repository<Promotion>, IPromotionRepository
    {
        protected new readonly ApplicationDbContext _context;

        public PromotionRepository(ApplicationDbContext context, ILogger<Repository<Promotion>> logger)
            : base(context, logger)
        {
            _context = context;
        }

        public async Task<bool> IsCodeUniqueAsync(string code)
        {
            return await _context.Promotions.AnyAsync(p => p.Code == code);
        }
        public async Task<Promotion> GetByCodeAsync(string code)
        {
            try
            {
                return await _context.Promotions
                   .FirstOrDefaultAsync(p => p.Code == code);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving promotion by code");
                throw;
            }
        }

        public async Task<IEnumerable<Promotion>> GetActivePromotionsAsync()
        {
            try
            {
                return await _context.Promotions
                   .Where(p => p.IsActive && p.ValidUntil >= DateTime.UtcNow)
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active promotions");
                throw;
            }
        }

        public async Task<IEnumerable<Promotion>> GetExpiredPromotionsAsync()
        {
            try
            {
                return await _context.Promotions
                   .Where(p => p.ValidUntil < DateTime.UtcNow)
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving expired promotions");
                throw;
            }
        }

        public async Task<IEnumerable<Promotion>> GetByRestaurantIdAsync(int restaurantId)
        {
            return await _context.Promotions
               .Where(p => p.RestaurantId == restaurantId)
               .ToListAsync();

        }

        public async Task<(bool isValid, string errorMessage)> IsPromotionValidAsync(string code, decimal orderAmount, int? restaurantId = null)
        {
            try
            {
                var promotion = await GetByCodeAsync(code);
                if (promotion == null) return (false, "Invalid promotion code.");

                if (!promotion.IsActive) return (false, "This promotion is no longer active.");

                if (promotion.ValidUntil < DateTime.UtcNow) return (false, "This promotion has expired.");

                if (orderAmount < promotion.MinimumOrderAmount) return (false, $"Minimum order amount for this promotion is {promotion.MinimumOrderAmount:C}.");

                if (promotion.RestaurantId.HasValue && promotion.RestaurantId != restaurantId) return (false, "This promotion is not applicable to the selected restaurant.");

                if (promotion.UsageCount >= promotion.MaxUsageLimit) return (false, "This promotion has reached its maximum usage limit.");

                return (true, null);
            }
            catch (Exception ex)
            {
                return (false, "Failed to validate promotion. Please try again.");
            }
        }
    }
}

