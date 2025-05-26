using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IMenuItemCategoryRepository : IRepository<MenuItemCategory>
    {
        Task<MenuItemCategory?> GetByNameAsync(string name);
    }
}
