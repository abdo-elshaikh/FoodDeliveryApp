using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Promotion;

namespace FoodDeliveryApp.ViewModels.Restaurant
{
    public class RestaurantMenuItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Discount { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public bool IsPopular { get; set; }
        public bool IsSpicy { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<MenuItemOptionViewModel> Options { get; set; } = new();
    }

    public class MenuItemOptionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsRequired { get; set; }
        public int MaxSelection { get; set; }
        public List<MenuItemOptionValueViewModel> Values { get; set; } = new();
    }

    public class MenuItemOptionValueViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsDefault { get; set; }
    }

    public class RestaurantCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int ItemCount { get; set; }
        public List<RestaurantMenuItemViewModel> MenuItems { get; set; } = new();
    }
    
    public class RestaurantCardViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string DeliveryTime { get; set; } = string.Empty;
        public decimal DeliveryFee { get; set; }
        public bool IsOpen { get; set; }
        public string[] Categories { get; set; } = Array.Empty<string>();
        public RestaurantAddressViewModel Address { get; set; } = new();
        public string CategoryName { get; set; } = string.Empty;
    }

    public class RestaurantViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public double Rating { get; set; }
        public int ReviewCount { get; set; }
        public string DeliveryTime { get; set; } = string.Empty;
        public string[] Categories { get; set; } = Array.Empty<string>();
        public string Website { get; set; } = string.Empty;
        public string LocationUrl { get; set; } = string.Empty;
        public decimal DeliveryFee { get; set; }
        public bool IsOpen { get; set; }
        public bool IsActive { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public RestaurantAddressViewModel Address { get; set; } = new();
        public string CategoryName { get; set; } = string.Empty;
        public string CuisineType { get; set; } = string.Empty;
        public string AverageDeliveryTime { get; set; } = string.Empty;
        public bool IsAdminOrOwner { get; set; }
        public decimal TaxRate { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public List<Review.RestaurantReviewViewModel> Reviews { get; set; } = new();
        public List<PromotionViewModel> Promotions { get; set; } = new();
    }
}