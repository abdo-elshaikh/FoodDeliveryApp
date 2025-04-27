using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IMenuItemRepository : IRepository<MenuItem>
    {
        Task<IEnumerable<MenuItem>> GetByRestaurantAsync(int restaurantId);
        Task<IEnumerable<MenuItem>> GetAvailableItemsByRestaurantAsync(int restaurantId);
        Task<IEnumerable<MenuItem>> GetPopularItemsAsync(int count, int restaurantId);
        Task<IEnumerable<MenuItem>> GetByRestaurantCategoryAsync(int categoryId);

        Task<List<MenuItem>> GetRelatedItemsAsync(int menuItemId, int restaurantId, int take);
        Task<List<CustomizationOption>> GetCustomizationOptionsAsync(int menuItemId);
        //GetPopularDishesAsync
        Task<IEnumerable<MenuItem>> GetPopularDishesAsync(int count);
    }
}
