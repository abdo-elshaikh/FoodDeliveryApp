using System.Collections.Generic;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.MenuItem;

namespace FoodDeliveryApp.ViewModels.Home
{
    public class SearchViewModel
    {
        public string Query { get; set; }
        public List<RestaurantCardViewModel> Restaurants { get; set; } = new List<RestaurantCardViewModel>();
        public List<RestaurantMenuItemViewModel> MenuItems { get; set; } = new List<RestaurantMenuItemViewModel>();
        public List<MenuItemCategoryViewModel> Categories { get; set; } = new List<MenuItemCategoryViewModel>();
    }
}
