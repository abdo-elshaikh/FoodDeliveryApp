using FoodDeliveryApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IRestaurantRepository : IRepository<Restaurant>
    {
        Task<IEnumerable<Restaurant>> GetRestaurantsByCategoryAsync(int categoryId);
        Task<IEnumerable<Restaurant>> SearchRestaurantsAsync(string searchTerm, string sortBy, int page, int pageSize);
        Task<int> GetSearchResultsCountAsync(string searchTerm);
        Task<IEnumerable<Restaurant>> GetActiveRestaurantsAsync();
        Task<Restaurant> GetRestaurantByIdAsync(int id);
        Task<IEnumerable<Restaurant>> GetRestaurantsByLocationAsync(string location);
        Task<Restaurant> GetRestaurantByNameAsync(string name);
        Task<IEnumerable<Restaurant>> GetTopRatedRestaurantsAsync(int count);
        Task<IEnumerable<Restaurant>> GetFeaturedRestaurantsAsync(int count = 6);
    }
}
