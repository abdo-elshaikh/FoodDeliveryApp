using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.MenuItem;
using FoodDeliveryApp.ViewModels.Review;

namespace FoodDeliveryApp.ViewModels.Restaurant
{
    public class RestaurantDetailsViewModel
    {
        public RestaurantViewModel Restaurant { get; set; } = new();
        public List<MenuItemViewModel> MenuItems { get; set; } = new();
        public List<RestaurantCategoryViewModel> Categories { get; set; } = new();
        public List<RestaurantReviewViewModel> Reviews { get; set; } = new();

    }
} 