using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.Review;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.MenuItems
{
    public class MenuItemListViewModel
    {
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();
        public IEnumerable<MenuItemCategoryViewModel> Categories { get; set; } = new List<MenuItemCategoryViewModel>();
        public IEnumerable<MenuItemRestaurantViewModel> Restaurants { get; set; } = new List<MenuItemRestaurantViewModel>(); 
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public string RestaurantImageUrl { get; set; } = string.Empty;
        public string SearchQuery { get; set; } = string.Empty;
        public int PageNumber { get; set; } = 1; // Current page
        public int PageSize { get; set; } = 12; // Items per page
        public int TotalItems { get; set; } // Total matching items
        public int? SelectedCategoryId { get; set; } // Optional category filter
        public int? SelectedRestaurantId { get; set; } // Optional restaurant filter
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> RestaurantOptions { get; set; } = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> CategoryOptions { get; set; } = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
        public string SortBy { get; set; } = "Name";
        public string SortOrder { get; set; } = "asc";
    }

    public class MenuItemCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
    }

    public class MenuItemRestaurantViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int MenuItemCount { get; set; }
    }

    public class MenuItemsByCategoryViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        // restaurants
        public IEnumerable<RestaurantViewModel> Restaurants { get; set; } = new List<RestaurantViewModel>();
        public IEnumerable<MenuItemViewModel> MenuItems { get; set; } = new List<MenuItemViewModel>();
    }

    public class MenuItemDetailsViewModel
    {
        public MenuItemViewModel MenuItem { get; set; } = new MenuItemViewModel();
        public RestaurantViewModel Restaurant { get; set; } = new RestaurantViewModel();
        public List<RelatedItemViewModel> RelatedItems { get; set; } = new List<RelatedItemViewModel>();
        public List<CustomizationOptionViewModel> CustomizationOptions { get; set; } = new List<CustomizationOptionViewModel>();
        public bool IsInUserCart { get; set; }
        public int? CartItemQuantity { get; set; }
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
    }

    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool IsSpicy { get; set; }
        public int? PrepTime { get; set; }
        public int? Calories { get; set; }
        public string? Ingredients { get; set; }
        public string? Tags { get; set; }
        public int? CategoryId { get; set; }
        public MenuItemCategoryViewModel Category { get; set; } = new MenuItemCategoryViewModel();
        public string RestaurantName { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
        public List<ReviewViewModel> Reviews { get; set; } = new List<ReviewViewModel>();
        public List<CustomizationOptionViewModel> CustomizationOptions { get; set; } = new List<CustomizationOptionViewModel>();
    }

    public class RelatedItemViewModel
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public decimal Rating { get; set; }
    }

    public class CustomizationOptionViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public bool IsRequired { get; set; }
        public bool AllowMultiple { get; set; }
        public List<CustomizationChoiceViewModel> Choices { get; set; } = new List<CustomizationChoiceViewModel>();
    }

    public class CustomizationChoiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public bool IsDefault { get; set; }
    }

    public class MenuItemCreateViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Menu Item Name")]
        public string Name { get; set; } = string.Empty;
        [StringLength(500)]
        [Display(Name = "Description")]
        public string? Description { get; set; }
        [Required]
        [Display(Name = "Price")]
        public decimal Price { get; set; } = 0.00M;
        [Display(Name = "Image")]
        public string? ImageUrl { get; set; }
        [Display(Name = "Upload Image")]
        [DataType(DataType.Upload)]
        public IFormFile? ImageFile { get; set; }
        [Display(Name = "Is Available")]
        [Required]
        public bool IsAvailable { get; set; } = true;
        [Required]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        [Display(Name = "Vegetarian")]
        public bool IsVegetarian { get; set; }
        [Display(Name = "Vegan")]
        public bool IsVegan { get; set; }
        [Display(Name = "Gluten-Free")]
        public bool IsGlutenFree { get; set; }
        [Display(Name = "Spicy")]
        public bool IsSpicy { get; set; }
        [Display(Name = "Preparation Time (minutes)")]
        public int? PrepTime { get; set; }
        [Display(Name = "Calories")]
        public int? Calories { get; set; }
        [Display(Name = "Ingredients (comma-separated)")]
        public string? Ingredients { get; set; }
        [Display(Name = "Tags (comma-separated)")]
        public string? Tags { get; set; }

        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> RestaurantOptions { get; set; } = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
        public IEnumerable<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem> CategoryOptions { get; set; } = new List<Microsoft.AspNetCore.Mvc.Rendering.SelectListItem>();
    }

    public class MenuItemEditViewModel : MenuItemCreateViewModel
    {
        public int Id { get; set; }
        public string? CurrentImageUrl { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }
    }
}
