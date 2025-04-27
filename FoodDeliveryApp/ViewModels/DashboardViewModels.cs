using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.OrderViewModels;
using FoodDeliveryApp.ViewModels.RestaurantViewModels;
using static FoodDeliveryApp.ViewModels.AccountViewModels;

namespace FoodDeliveryApp.ViewModels
{
    public class DashboardViewModels
    {
        public class CustomerDashboardViewModel
        {
            public ProfileViewModel Profile { get; set; }
            public List<OrderViewModel> RecentOrders { get; set; } = new List<OrderViewModel>();
            public List<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();
            public List<PaymentMethodViewModel> PaymentMethods { get; set; } = new List<PaymentMethodViewModel>();
            public int LoyaltyPoints { get; set; }
        }

        public class RestaurantDashboardViewModel
        {
            public RestaurantViewModel Restaurant { get; set; }
            public List<OrderViewModel> RecentOrders { get; set; } = new List<OrderViewModel>();
            public int TotalOrders { get; set; }
            public decimal TotalRevenue { get; set; }
            public List<ReviewViewModel> RecentReviews { get; set; } = new List<ReviewViewModel>();
            public decimal AverageRating { get; set; }
        }

        public class AdminDashboardViewModel
        {
            public int TotalUsers { get; set; }
            public int TotalRestaurants { get; set; }
            public int TotalOrdersToday { get; set; }
            public decimal RevenueToday { get; set; }
            public List<RestaurantViewModel> NewRestaurants { get; set; } = new List<RestaurantViewModel>();
            public List<OrderViewModel> RecentOrders { get; set; } = new List<OrderViewModel>();
            public Dictionary<string, decimal> SalesByCategory { get; set; } = new Dictionary<string, decimal>();
            public Dictionary<int, int> PopularItems { get; set; } = new Dictionary<int, int>();
        }

        public class OrderReportViewModel
        {
            // total orders, total revenue, average order value, most popular items
            public int TotalOrders { get; set; }
            public decimal TotalRevenue { get; set; }
            public decimal AverageOrderValue { get; set; }
            public List<OrderItemViewModel> MostPopularItems { get; set; } = new List<OrderItemViewModel>();
            public List<OrderViewModel> RecentOrders { get; set; } = new List<OrderViewModel>();
        }

        public class UserViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string FullName { get; set; }
            public UserRole Role { get; set; }
            public bool IsActive { get; set; }
            public bool EmailConfirmed { get; set; }
        }
    }
}
