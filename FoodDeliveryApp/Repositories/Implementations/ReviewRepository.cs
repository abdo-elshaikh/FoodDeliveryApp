using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class ReviewRepository : Repository<Review>, IReviewRepository
    {
        private readonly ApplicationDbContext _context;
        public ReviewRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task AddReviewAsync(Review review)
        {
            await _context.Reviews.AddAsync(review);

            // Update Restaurant.Rating
            var restaurant = await _context.Restaurants
                .Include(r => r.Reviews)
                .FirstOrDefaultAsync(r => r.Id == review.RestaurantId);

            if (restaurant != null)
            {
                restaurant.Rating = restaurant.Reviews.Any() ? restaurant.Reviews.Average(r => r.Rating) : 0m;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<Review>> GetByRestaurantAsync(int restaurantId)
            => await _context.Reviews
                .Include(r => r.CustomerProfile)
                .Where(r => r.RestaurantId == restaurantId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public async Task<IEnumerable<Review>> GetByCustomerAsync(int customerId)
            => await _context.Reviews
                .Include(r => r.Restaurant)
                .Where(r => r.CustomerProfileId == customerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();

        public async Task<double?> GetAverageRatingAsync(int restaurantId)
        {
            return await _context.Reviews
                .Where(r => r.RestaurantId == restaurantId)
                .Select(r => (double?)r.Rating)
                .AverageAsync();
        }

        public async Task<IEnumerable<Review>> GetRecentReviewsAsync(int count)
            => await _context.Reviews
                .Include(r => r.Restaurant)
                .Include(r => r.CustomerProfile)
                .OrderByDescending(r => r.CreatedAt)
                .Take(count)
                .ToListAsync();
    }
}
