using System.Collections.Generic;

namespace FoodDeliveryApp.ViewModels.Home
{
    public class HomeViewModel : ViewModelBase
    {
        public HeroSection Hero { get; set; }
        public List<HomeCategoryVM> Categories { get; set; } = new List<HomeCategoryVM>();
        public List<FeaturedDishViewModel> FeaturedDishes { get; set; } = new List<FeaturedDishViewModel>();
        public AppDownloadSection AppDownload { get; set; }
        public List<string> PopularSearches { get; set; } = new List<string>();
        public List<PromotionViewModel> ActivePromotions { get; set; } = new List<PromotionViewModel>();
        public List<HomeRestaurantViewModel> TopRatedRestaurants { get; set; } = new List<HomeRestaurantViewModel>();
        // Search
        public SearchViewModel Search { get; set; }
    }


    // Search
    public class SearchViewModel
    {
        public List<string> PopularSearches { get; set; } = new List<string>();
    }


    public class HeroSection
    {
        public string Title { get; set; } = "Order Food Online from Your Favorite Restaurants";
        public string Subtitle { get; set; } = "Fast delivery, great deals, and a wide variety of cuisines at your fingertips.";
        public string BackgroundImageUrl { get; set; }
    }

    public class HomeCategoryVM
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Icon { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
        public int RestaurantCount { get; set; }
    }

    public class FeaturedDishViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public string RestaurantName { get; set; }
        public int RestaurantId { get; set; }
        public double Rating { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class AppDownloadSection
    {
        public string Title { get; set; } = "Get the FoodFast App";
        public string Description { get; set; } = "Download our mobile app for a better experience and exclusive deals.";
        public string AppStoreUrl { get; set; } = "#";
        public string PlayStoreUrl { get; set; } = "#";
        public string AppScreenshotUrl { get; set; }
        public List<AppFeature> Features { get; set; } = new List<AppFeature>
        {
            new AppFeature { Title = "Real-time tracking", Icon = "bi bi-geo-alt" },
            new AppFeature { Title = "Exclusive deals", Icon = "bi bi-tag" },
            new AppFeature { Title = "Faster ordering", Icon = "bi bi-lightning" },
            new AppFeature { Title = "Loyalty rewards", Icon = "bi bi-star" }
        };
    }

    public class AppFeature
    {
        public string Title { get; set; }
        public string Icon { get; set; }
    }

    public class PromotionViewModel
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public decimal DiscountValue { get; set; }
        public bool IsPercentage { get; set; }
        public decimal MinimumOrderAmount { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int? RestaurantId { get; set; }
        public string RestaurantName { get; set; }
    }

    public class HomeRestaurantViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string ImageUrl { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string CategoryName { get; set; }
        public decimal DeliveryFee { get; set; }
        public TimeSpan EstimatedDeliveryTime { get; set; }
        public bool IsOpen { get; set; }
        public List<string> Tags { get; set; }
    }
}
