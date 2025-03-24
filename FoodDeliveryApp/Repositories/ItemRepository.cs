using FoodDeliveryApp.Models;
using FoodDeliveryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories
{
    public class ItemRepository : Repository<Item>, IItemRepository
    {
        private readonly ApplicationDbContext _context;
        public ItemRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        
        public Item GetByItemId(int itemId)
        {
            return _context.Items.Include(i => i.Category).FirstOrDefault(i => i.ItemId == itemId);
        }
    }
}
