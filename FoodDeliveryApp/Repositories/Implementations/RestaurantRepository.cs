using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Linq;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        protected new readonly ApplicationDbContext _context;
        protected new readonly ILogger<Repository<Restaurant>> _logger;

        public RestaurantRepository(
            ApplicationDbContext context,
            ILogger<RestaurantRepository> logger) : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task<IEnumerable<Restaurant>> GetFeaturedRestaurantsAsync(int count = 6)
        {
            try
            {
                var restaurants = await _context.Restaurants
                    .Where(r => r.IsActive)
                    .Include(r => r.Reviews)
                    .Include(r => r.Categories)
                    .OrderByDescending(r => r.Rating)
                    .Take(count)
                    .ToListAsync();

                return restaurants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving featured restaurants");
                throw;
            }
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsByCategoryAsync(int categoryId)
        {
            try
            {
                var restaurants = await _context.Restaurants
                    .Where(r => r.CategoryId == categoryId && r.IsActive)
                    .Include(r => r.Reviews)
                    .Include(r => r.Categories)
                    .ToListAsync();

                return restaurants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurants for category {CategoryId}", categoryId);
                throw;
            }
        }

        public async Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string searchTerm, string sortBy, int page, int pageSize)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    _logger.LogWarning("Search term is null or empty");
                    return Enumerable.Empty<Restaurant>();
                }

                IQueryable<Restaurant> query = _context.Restaurants
                    .Where(r => r.IsActive && (r.Name.Contains(searchTerm) || r.Description.Contains(searchTerm)))
                    .Include(r => r.Reviews)
                    .Include(r => r.Categories);

                // Apply sorting
                query = sortBy.ToLower() switch
                {
                    "rating" => query.OrderByDescending(r => r.Rating),
                    "delivery_time" => query.OrderBy(r => r.DeliveryTime),
                    "price_low_to_high" => query.OrderBy(r => r.DeliveryFee),
                    "price_high_to_low" => query.OrderByDescending(r => r.DeliveryFee),
                    _ => query.OrderBy(r => r.Name)
                };

                var restaurants = await query
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return restaurants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching restaurants with term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<int> GetSearchResultsCountAsync(string searchTerm)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchTerm))
                {
                    return 0;
                }

                return await _context.Restaurants
                    .CountAsync(r => r.IsActive && (r.Name.Contains(searchTerm) || r.Description.Contains(searchTerm)));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting search results count for term {SearchTerm}", searchTerm);
                throw;
            }
        }

        public async Task<IEnumerable<Restaurant>> GetActiveRestaurantsAsync()
        {
            try
            {
                var restaurants = await _context.Restaurants
                    .Where(r => r.IsActive)
                    .Include(r => r.Reviews)
                    .Include(r => r.Categories)
                    .ToListAsync();

                return restaurants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving active restaurants");
                throw;
            }
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            try
            {
                var restaurant = await _context.Restaurants
                    .Include(r => r.Category)
                    .Include(r => r.Reviews)
                    .Include(r => r.Categories)
                    .FirstOrDefaultAsync(r => r.Id == id);

                if (restaurant == null)
                {
                    _logger.LogWarning("Restaurant with ID {RestaurantId} not found", id);
                }

                return restaurant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurant with ID {RestaurantId}", id);
                throw;
            }
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAsync(string location)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(location))
                {
                    _logger.LogWarning("Location search term is null or empty");
                    return Enumerable.Empty<Restaurant>();
                }

                var restaurants = await _context.Restaurants
                    .Where(r => r.IsActive && r.Address.Contains(location))
                    .Include(r => r.Category)
                    .Include(r => r.Reviews)
                    .Include(r => r.Categories)
                    .ToListAsync();

                return restaurants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurants for location {Location}", location);
                throw;
            }
        }

        public async Task<Restaurant> GetRestaurantByNameAsync(string name)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(name))
                {
                    _logger.LogWarning("Restaurant name is null or empty");
                    return null;
                }

                var restaurant = await _context.Restaurants
                    .Include(r => r.Category)
                    .Include(r => r.Reviews)
                    .Include(r => r.Categories)
                    .FirstOrDefaultAsync(r => r.Name == name);

                if (restaurant == null)
                {
                    _logger.LogWarning("Restaurant with name {RestaurantName} not found", name);
                }

                return restaurant;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving restaurant with name {RestaurantName}", name);
                throw;
            }
        }

        public async Task<IEnumerable<Restaurant>> GetTopRatedRestaurantsAsync(int count)
        {
            try
            {
                if (count <= 0)
                {
                    _logger.LogWarning("Invalid count value: {Count}", count);
                    return Enumerable.Empty<Restaurant>();
                }

                var restaurants = await _context.Restaurants
                    .Where(r => r.IsActive)
                    .Include(r => r.Reviews)
                    .Include(r => r.Categories)
                    .Include(r => r.Category)
                    .OrderByDescending(r => r.Rating)
                    .Take(count)
                    .ToListAsync();

                return restaurants;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving top {Count} rated restaurants", count);
                throw;
            }
        }
    }
}
