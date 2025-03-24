using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Category GetByCategoryId(int categoryId);
        Category GetWithItems(int catId);
    }
}
