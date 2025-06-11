using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.MenuItem;
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

    public class SelectListItem
    {
        public string Value { get; set; } = string.Empty;
        public string Text { get; set; } = string.Empty;
        public bool Selected { get; set; }
    }
} 