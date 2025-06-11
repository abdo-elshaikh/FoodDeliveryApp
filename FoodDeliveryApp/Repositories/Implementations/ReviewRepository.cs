using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        protected new readonly ApplicationDbContext _context;
        protected new readonly ILogger<Repository<Review>> _logger;

        public ReviewRepository(
            ApplicationDbContext context,
            ILogger<ReviewRepository> logger) : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<double?> GetAverageRatingAsync(int? restaurantId, int? MenuItemId)
        {
            if (restaurantId == null || MenuItemId == null)
            {
                return null;
            }

            var reviews = await _context.Reviews.Where(r => r.RestaurantId == restaurantId || r.MenuItemId == MenuItemId).ToListAsync();
            return reviews.Any() ? (double)reviews.Average(r => r.Rating) : null;
        }

        public async Task<IEnumerable<Review>> GetByMenuItemAsync(int MenuItemId)
        {
            return await _context.Reviews.Where(r => r.MenuItemId == MenuItemId).Include(r => r.User).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByRestaurantAsync(int restaurantId)
        {
            return await _context.Reviews.Where(r => r.RestaurantId == restaurantId).Include(r => r.User).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetByUserIdAsync(string userId)
        {
            return await _context.Reviews.Where(r => r.UserId == userId).Include(r => r.User).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetRecentReviewsAsync(int count)
        {
            return await _context.Reviews.Include(r => r.User).OrderByDescending(r => r.CreatedAt).Take(count).ToListAsync();
        }

        public async Task<IEnumerable<Review>> GetRestaurantReviewsAsync(int restaurantId)
        {
            return await _context.Reviews.Where(r => r.RestaurantId == restaurantId).Include(r => r.User).ToListAsync();
        }
    }
}
