using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Category;
using FoodDeliveryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Services
{
    public class CategoryService : ICategoryService
    {
        private readonly ApplicationDbContext _context;

        public CategoryService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<CategoryViewModel>> GetPopularCategoriesAsync()
        {
            return await _context.RestaurantCategories
                .OrderByDescending(c => c.Restaurants.Count)
                .Take(8)
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl ?? "/images/default-category.png",
                    RestaurantCount = c.Restaurants.Count
                })
                .ToListAsync();
        }

        public async Task<CategoryViewModel> GetCategoryByIdAsync(int id)
        {
            var category = await _context.RestaurantCategories
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
                return null;

            return new CategoryViewModel
            {
                Id = category.Id,
                Name = category.Name,
                ImageUrl = category.ImageUrl ?? "/images/default-category.png",
                RestaurantCount = category.Restaurants.Count
            };
        }

        public async Task<List<CategoryViewModel>> GetAllCategoriesAsync()
        {
            return await _context.RestaurantCategories
                .OrderBy(c => c.Name)
                .Select(c => new CategoryViewModel
                {
                    Id = c.Id,
                    Name = c.Name,
                    ImageUrl = c.ImageUrl ?? "/images/default-category.png",
                    RestaurantCount = c.Restaurants.Count
                })
                .ToListAsync();
        }
    }
} 