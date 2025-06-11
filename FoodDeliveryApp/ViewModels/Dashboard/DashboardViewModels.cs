using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.MenuItem;
using FoodDeliveryApp.ViewModels.Order;
using FoodDeliveryApp.ViewModels.Promotion;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.Review;

namespace FoodDeliveryApp.ViewModels.Dashboard
{
    public class DashboardViewModel
    {
        public UserDashboardViewModel UserDashboard { get; set; } = new();
        public RestaurantDashboardViewModel? RestaurantDashboard { get; set; }
        public DeliveryDriverDashboardViewModel? DeliveryDriverDashboard { get; set; }
        public AdminDashboardViewModel? AdminDashboard { get; set; }
    }

    public class UserDashboardViewModel
    {
        public List<OrderSummaryViewModel> RecentOrders { get; set; } = new();
        public List<RestaurantListViewModel> FavoriteRestaurants { get; set; } = new();
        public List<MenuItem.MenuItemViewModel> RecentlyOrderedItems { get; set; } = new();
        public List<AddressViewModel> SavedAddresses { get; set; } = new();
        public List<ReviewListViewModel> RecentReviews { get; set; } = new();
        public UserStatsViewModel UserStats { get; set; } = new();
    }

    public class RestaurantDashboardViewModel
    {
        public RestaurantStatsViewModel RestaurantStats { get; set; } = new();
        public List<OrderSummaryViewModel> RecentOrders { get; set; } = new();
        public List<MenuItem.MenuItemViewModel> PopularItems { get; set; } = new();
        public List<ReviewListViewModel> RecentReviews { get; set; } = new();
        public List<MenuItem.MenuItemViewModel> LowStockItems { get; set; } = new();
        public List<MenuItem.MenuItemViewModel> OutOfStockItems { get; set; } = new();
        public List<PromotionViewModel> ActivePromotions { get; set; } = new();
    }

    public class DeliveryDriverDashboardViewModel
    {
        public DeliveryDriverStatsViewModel DriverStats { get; set; } = new();
        public List<OrderSummaryViewModel> ActiveDeliveries { get; set; } = new();
        public List<OrderSummaryViewModel> CompletedDeliveries { get; set; } = new();
        public List<ReviewListViewModel> RecentReviews { get; set; } = new();
    }

    public class AdminDashboardViewModel
    {
        public AdminStatsViewModel AdminStats { get; set; } = new();
        public List<RestaurantListViewModel> NewRestaurants { get; set; } = new();
        public List<UserSummaryViewModel> NewUsers { get; set; } = new();
        public List<OrderSummaryViewModel> RecentOrders { get; set; } = new();
        public List<ReviewListViewModel> RecentReviews { get; set; } = new();
        public List<PromotionViewModel> ActivePromotions { get; set; } = new();
    }

    public class UserStatsViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalSpent { get; set; }
        public int FavoriteRestaurantsCount { get; set; }
        public int ReviewsCount { get; set; }
        public double AverageRating { get; set; }
    }

    public class RestaurantStatsViewModel
    {
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActiveMenuItems { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public int PendingOrders { get; set; }
        public int CompletedOrders { get; set; }
        public int CancelledOrders { get; set; }
    }

    public class DeliveryDriverStatsViewModel
    {
        public int TotalDeliveries { get; set; }
        public decimal TotalEarnings { get; set; }
        public int ActiveDeliveries { get; set; }
        public int CompletedDeliveries { get; set; }
        public int TotalReviews { get; set; }
        public double AverageRating { get; set; }
        public decimal AverageDeliveryTime { get; set; }
    }

    public class AdminStatsViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalRestaurants { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActivePromotions { get; set; }
        public int PendingApprovals { get; set; }
        public int TotalReviews { get; set; }
    }

   
    public class UserSummaryViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
    }
} 