using FoodDeliveryApp.ViewModels.RestaurantViewModels;
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
        public MenuItemViewModel MenuItem { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
        public List<RelatedItemViewModel> RelatedItems { get; set; }
        public List<CustomizationOptionViewModel> CustomizationOptions { get; set; }
    }

    public class MenuItemViewModel
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public bool IsAvailable { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public int? CategoryId { get; set; }
        public MenuItemCategoryViewModel Category { get; set; } = new MenuItemCategoryViewModel();

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
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public bool AllowMultiple { get; set; }
        public List<CustomizationChoiceViewModel> Choices { get; set; }
    }

    public class CustomizationChoiceViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsDefault { get; set; }
    }

    public class MenuItemCreateViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Menu Item Name")]
        public string Name { get; set; }
        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }
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
        public int RestaurantId { get; set; }
    }

    public class MenuItemEditViewModel : MenuItemCreateViewModel
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        [Display(Name = "Updated At")]
        public DateTime? UpdatedAt { get; set; }
    }
}
