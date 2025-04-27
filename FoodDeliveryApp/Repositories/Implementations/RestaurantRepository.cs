using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class RestaurantRepository : Repository<Restaurant>, IRestaurantRepository
    {
        private readonly ApplicationDbContext _context;
        public RestaurantRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsByCategoryAsync(int categoryId)
        {
            return await _context.Restaurants
                .Where(r => r.CategoryId == categoryId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string searchTerm)
        {
            return await _context.Restaurants
                .Where(r => r.Name.Contains(searchTerm) || r.Description.Contains(searchTerm))
                .ToListAsync();
        }

        public async Task<IEnumerable<Restaurant>> GetActiveRestaurantsAsync()
        {
            return await _context.Restaurants
                .Where(r => r.IsActive)
                .ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByIdAsync(int id)
        {
            return await _context.Restaurants
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAsync(string location)
        {
            return await _context.Restaurants
                .Where(r => r.Address.Contains(location) || r.City.Contains(location))
                .Include(r => r.Category)
                .ToListAsync();
        }

        public async Task<Restaurant> GetRestaurantByNameAsync(string name)
        {
            return await _context.Restaurants
                .Include(r => r.Category)
                .FirstOrDefaultAsync(r => r.Name == name);
        }

        public async Task<IEnumerable<Restaurant>> GetTopRatedRestaurantsAsync(int count)
        {
            return await _context.Restaurants
            .Include(r => r.Reviews)
            .Include(r => r.Category)
            .OrderByDescending(r => r.Rating)
            .Take(count)
            .ToListAsync();
        }
    }
}
