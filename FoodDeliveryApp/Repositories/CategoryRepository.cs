using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Data;

namespace FoodDeliveryApp.Repositories
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ApplicationDbContext _context;
        public CategoryRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Category GetByCategoryId(int cateqId)
        {
            return _context.Categories.FirstOrDefault(c => c.CateqId == cateqId);
        }

        // GetWithItems method is used to get the category with its items
        public Category GetWithItems(int catId)
        {
            return _context.Categories.Include(c => c.Items).FirstOrDefault(c => c.CateqId == catId);
        }
    }
}
