using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        private readonly ApplicationDbContext _context;
        public MenuItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<MenuItem>> GetByRestaurantAsync(int restaurantId)
            => await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId)
                .OrderBy(m => m.Name)
                .ToListAsync();

        public async Task<IEnumerable<MenuItem>> GetAvailableItemsByRestaurantAsync(int restaurantId)
            => await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
                .OrderBy(m => m.Name)
                .ToListAsync();

        public async Task<IEnumerable<MenuItem>> GetPopularItemsAsync(int count, int restaurantId)
            => await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId && m.IsAvailable)
                .OrderByDescending(m => m.OrderItems.Count())
                .Take(count)
                .ToListAsync();

        public async Task<IEnumerable<MenuItem>> GetByRestaurantCategoryAsync(int categoryId)
        {
            var restaurants = await _context.Restaurants
                .Where(r => r.CategoryId == categoryId)
                .Select(r => r.Id)
                .ToListAsync();

            // Get all menu items for the restaurants in the specified category
            var menuItems = await _context.MenuItems
                .Where(m => restaurants.Contains(m.RestaurantId) && m.IsAvailable)
                .ToListAsync();
            return menuItems;
        }

        public async Task<List<CustomizationOption>> GetCustomizationOptionsAsync(int menuItemId)
        {
            // Example: Fetch from DB (e.g., EF Core)
            return await _context.CustomizationOptions
                .Where(o => o.MenuItemId == menuItemId)
                .Include(o => o.Choices)
                .ToListAsync();
        }

        public async Task<List<MenuItem>> GetRelatedItemsAsync(int menuItemId, int restaurantId, int take)
        {
            // Example: Fetch related items by restaurant, excluding current item
            return await _context.MenuItems
                .Where(m => m.RestaurantId == restaurantId && m.Id != menuItemId && m.IsAvailable)
                .OrderBy(m => m.Price)
                .Take(take)
                .ToListAsync();
        }

        //GetPopularDishesAsync
        public async Task<IEnumerable<MenuItem>> GetPopularDishesAsync(int count)
        {
            return await _context.MenuItems
                .Where(m => m.IsAvailable)
                .OrderByDescending(m => m.OrderItems.Count())
                .Take(count)
                .ToListAsync();
        }

        //SearchMenuItemsAsync
        public async Task<IEnumerable<MenuItem>> SearchMenuItemsAsync(string searchQuery, int? restaurantId, int? categoryId, int pageNumber, int pageSize)
        {
            var query = _context.MenuItems.AsQueryable();
            if (!string.IsNullOrEmpty(searchQuery))
            {
                query = query.Where(m => m.Name.Contains(searchQuery) || m.Description.Contains(searchQuery));
            }
            if (restaurantId.HasValue)
            {
                query = query.Where(m => m.RestaurantId == restaurantId.Value);
            }
            if (categoryId.HasValue)
            {
                query = query.Where(m => m.Restaurant.CategoryId == categoryId.Value);
            }
            return await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }
    }
}