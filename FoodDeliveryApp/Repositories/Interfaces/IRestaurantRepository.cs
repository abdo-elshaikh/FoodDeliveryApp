using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{

    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<IEnumerable<Restaurant>> GetRestaurantsByCategoryAsync(int categoryId);
        Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string searchTerm);
        Task<IEnumerable<Restaurant>> GetActiveRestaurantsAsync();
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<Restaurant> GetRestaurantByNameAsync(string name);
        Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAsync(string location);
        // GetTopRatedRestaurantsAsync
        Task<IEnumerable<Restaurant>> GetTopRatedRestaurantsAsync(int count);
    }
}
