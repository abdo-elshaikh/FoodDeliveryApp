using FoodDeliveryApp.ViewModels.MenuItem;
using FoodDeliveryApp.ViewModels.Review;
using FoodDeliveryApp.ViewModels.Promotion;

namespace FoodDeliveryApp.ViewModels.Restaurant
{
    public class RestaurantDetailViewModel
    {
        public RestaurantViewModel Restaurant { get; set; } = new RestaurantViewModel();
        public List<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();
        public List<RestaurantReviewViewModel> Reviews { get; set; } = new List<RestaurantReviewViewModel>();
        public List<PromotionViewModel> Promotions { get; set; } = new List<PromotionViewModel>();
        public List<RestaurantCategoryViewModel> Categories { get; set; } = new List<RestaurantCategoryViewModel>();
        public bool IsAdminOrOwner { get; set; }
    }
}
