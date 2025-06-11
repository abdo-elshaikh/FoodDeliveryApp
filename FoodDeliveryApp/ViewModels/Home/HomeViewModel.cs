using System.Collections.Generic;

namespace FoodDeliveryApp.ViewModels.Home
{
    public class HomeViewModelBase
    {
        public List<RestaurantHomeViewModel> FeaturedRestaurants { get; set; } = new();
        public List<CategoryHomeViewModel> PopularCategories { get; set; } = new();
    }

    public class RestaurantHomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Restaurant name here.";
        public string ImageUrl { get; set; } = "restaurant-image.jpg";
        public string CuisineType { get; set; } = "Cuisine type here.";
        // description is optional
        public string Description { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int DeliveryTime { get; set; }
        public decimal DeliveryFee { get; set; }
    }

    public class CategoryHomeViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = "Category name here.";
        public string Icon { get; set; } = "fa fa-circle";
        public int RestaurantCount { get; set; }
        // imageUrl is optional
        public string ImageUrl { get; set; } = string.Empty;
    }

    public class HomeViewModel : HomeViewModelBase
    {
        public List<PromotionHomeViewModel> ActivePromotions { get; set; } = new();
        public List<ReviewHomeViewModel> RecentReviews { get; set; } = new();
        public List<RestaurantHomeViewModel> TrendingRestaurants { get; set; } = new();
        public List<CategoryHomeViewModel> FeaturedCategories { get; set; } = new();
        public UserLocationViewModel UserLocation { get; set; } = new();
    }

    public class PromotionHomeViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = "Promotion title here.";
        public string Description { get; set; } = "Promotion description here.";
        public string ImageUrl { get; set; } = "promotion-image.jpg";
        public string Code { get; set; } = string.Empty;
        public decimal DiscountAmount { get; set; }
        public string DiscountType { get; set; } = string.Empty;
        public System.DateTime ExpiryDate { get; set; }
        public bool IsActive { get; set; }
    }

    public class ReviewHomeViewModel
    {
        public int Id { get; set; }
        public string UserName { get; set; } = "User";
        public string UserAvatar { get; set; } = "user-avatar.jpg";
        public int Rating { get; set; }
        public string Comment { get; set; } = "Review content here.";
        public string RestaurantName { get; set; } = "Restaurant Name";
        public DateTime CreatedAt { get; set; }
        public List<string> Images { get; set; } = new();
    }

    public class UserLocationViewModel
    {
        public string Address { get; set; } = "User's Address";
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public string City { get; set; } = "User's City";
        public string State { get; set; } = "User's State";
        public string ZipCode { get; set; } = "User's Zip Code";
        public bool IsDeliveryAvailable { get; set; }
    }
} 