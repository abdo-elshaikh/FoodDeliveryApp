using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.MenuItems;
using FoodDeliveryApp.ViewModels.Restaurant;

namespace FoodDeliveryApp.ViewModels.Menu
{
    public class MenuViewModel
    {
        public List<MenuItemViewModel> MenuItems { get; set; } = new();
        public List<MenuItemCategoryViewModel> Categories { get; set; } = new();
        public List<RestaurantViewModel> Restaurants { get; set; } = new();
        public int? SelectedCategoryId { get; set; }
        public int? SelectedRestaurantId { get; set; }
        public string SearchQuery { get; set; } = string.Empty;
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
        public int TotalItems { get; set; }
        public int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public bool HasPreviousPage => PageNumber > 1;
        public bool HasNextPage => PageNumber < TotalPages;
    }

    public class MenuCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public int MenuItemCount { get; set; }
        public List<MenuItemViewModel> MenuItems { get; set; } = new();
        public List<RestaurantViewModel> Restaurants { get; set; } = new();
    }

    public class MenuRestaurantViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public string? ImageUrl { get; set; }
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public string? Cuisine { get; set; }
        public string? Address { get; set; }
        public bool IsOpen { get; set; }
        public TimeSpan? OpeningTime { get; set; }
        public TimeSpan? ClosingTime { get; set; }
        public decimal DeliveryFee { get; set; }
        public int MinimumOrder { get; set; }
        public int EstimatedDeliveryTime { get; set; }
        public List<MenuItemViewModel> MenuItems { get; set; } = new();
        public List<MenuItemCategoryViewModel> Categories { get; set; } = new();
    }

    public class MenuSearchViewModel
    {
        public string Query { get; set; } = string.Empty;
        public int? CategoryId { get; set; }
        public int? RestaurantId { get; set; }
        public decimal? MinPrice { get; set; }
        public decimal? MaxPrice { get; set; }
        public bool? IsVegetarian { get; set; }
        public bool? IsVegan { get; set; }
        public bool? IsGlutenFree { get; set; }
        public bool? IsSpicy { get; set; }
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 12;
    }

    public class MenuFilterViewModel
    {
        public List<MenuItemCategoryViewModel> Categories { get; set; } = new();
        public List<RestaurantViewModel> Restaurants { get; set; } = new();
        public decimal MinPrice { get; set; }
        public decimal MaxPrice { get; set; }
        public bool ShowVegetarian { get; set; }
        public bool ShowVegan { get; set; }
        public bool ShowGlutenFree { get; set; }
        public bool ShowSpicy { get; set; }
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
    }

    public class MenuSortViewModel
    {
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
        public List<SelectListItem> SortOptions { get; set; } = new()
        {
            new SelectListItem { Value = "Name", Text = "Name" },
            new SelectListItem { Value = "Price", Text = "Price" },
            new SelectListItem { Value = "Rating", Text = "Rating" },
            new SelectListItem { Value = "Popularity", Text = "Popularity" }
        };
    }

    public class SelectListItem
    {
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool Selected { get; set; }
    }
} 