using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Restaurant;

namespace FoodDeliveryApp.Services
{
    public interface IRestaurantService
    {
        Task<List<RestaurantViewModel>> GetFeaturedRestaurantsAsync();
        Task<RestaurantViewModel> GetRestaurantByIdAsync(int id);
        Task<List<RestaurantViewModel>> SearchRestaurantsAsync(string query, string location);
    }
} 