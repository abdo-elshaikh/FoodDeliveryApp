using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public interface IItemRepository : IRepository<Item>
    {
        Item GetByItemId(int itemId);
    }
}
