using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class MenuItemCategoryRepository : Repository<MenuItemCategory>, IMenuItemCategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public MenuItemCategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<MenuItemCategory?> GetByNameAsync(string name)
        {
            return await _context.MenuItemCategories.FirstOrDefaultAsync(c => c.Name == name);
        }
    }
}
