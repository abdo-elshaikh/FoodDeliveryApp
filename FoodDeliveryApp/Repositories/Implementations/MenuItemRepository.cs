﻿using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class MenuItemRepository : Repository<MenuItem>, IMenuItemRepository
    {
        protected new readonly ApplicationDbContext _context;
        protected new readonly ILogger<Repository<MenuItem>> _logger;

        public MenuItemRepository(ApplicationDbContext context, ILogger<MenuItemRepository> logger) : base(context, logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<MenuItem>> GetByRestaurantIdAsync(int restaurantId)
        {
            try
            {
                return await _context.MenuItems
                    .Where(m => m.RestaurantId == restaurantId)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu items by restaurant ID");
                throw;
            }
        }

        public async Task<IEnumerable<MenuItem>> GetByCategoryIdAsync(int categoryId)
        {
            try
            {
                return await _context.MenuItems
                    .Where(m => m.CategoryId == categoryId)
                    .ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu items by category ID");
                throw;
            }
        }

        public async Task<IEnumerable<MenuItem>> GetPopularItemsAsync(int count = 8)
        {
            try
            {
                return await _context.MenuItems
                   .OrderByDescending(m => m.Id)
                   .Take(count)
                   .ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving popular menu items");
                throw;
            }
        }

        public async Task<IEnumerable<MenuItemCategory>> GetPopularCategoriesAsync(int count = 6)
        {
            try
            {
                return await _context.MenuItemCategories
                    .OrderByDescending(c => c.MenuItems.Count)
                    .Take(count)
                    .ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving popular menu item categories");
                throw;
            }
        }

        public async Task<MenuItem> GetByIdWithDetailsAsync(int id)
        {
            try
            {
                return await _context.MenuItems
                    .Include(m => m.Restaurant)
                    .Include(m => m.Category)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item by ID with details");
                throw;
            }
        }

        public async Task<bool> UpdateAvailabilityAsync(int id, bool isAvailable)
        {
            try
            {
                var menuItem = await _context.MenuItems.FindAsync(id);
                if (menuItem == null)
                {
                    return false;
                }

                menuItem.IsAvailable = isAvailable;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error updating menu item availability");
                throw;
            }
        }

        public async Task<IEnumerable<MenuItem>> SearchMenuItemsAsync(string searchTerm)
        {
            try
            {
                return await _context.MenuItems
                   .Where(m => m.Name.Contains(searchTerm) || m.Description.Contains(searchTerm))
                   .ToListAsync();
            }
            catch (System.Exception ex)
            {
                _logger.LogError(ex, "Error searching menu items");
                throw;
            }
        }

        public async Task<IEnumerable<MenuItem>> GetByIdsAsync(IEnumerable<int> ids)
        {
            return await _context.MenuItems
                .Where(m => ids.Contains(m.Id))
                .ToListAsync();
        }

        public async Task<MenuItem> GetByIdWithRestaurantAsync(int id)
        {
            try
            {
                return await _context.MenuItems
                    .Include(m => m.Restaurant)
                    .FirstOrDefaultAsync(m => m.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving menu item by ID with restaurant details");
                throw;
            }
        }
    }
}
