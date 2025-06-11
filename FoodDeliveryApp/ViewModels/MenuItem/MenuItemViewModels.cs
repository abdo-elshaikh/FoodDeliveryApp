using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.Review;

namespace FoodDeliveryApp.ViewModels.MenuItem
{
    
    public class MenuItemViewModel : BaseViewModel
    {
        public new int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal OriginalPrice { get; set; }
        public decimal Discount { get; set; }
        public string ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public int? CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double Rating { get; set; }
        public TimeSpan PreparationTime { get; set; }
        public int Calories { get; set; }
        public int SpiceLevel { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsFavorite { get; set; }
        //tags
        public List<string> Ingredients { get; set; } = new List<string>();
        public List<string> Tags { get; set; } = new List<string>();
        public int ReviewCount { get; set; }
        public bool IsSpicy => SpiceLevel > 5;
        public bool IsHealthy => !IsSpicy && !IsVegetarian && !IsVegan;
        public bool IsPopular => Rating > 4.5;
        public bool IsNew => CreatedAt > DateTime.UtcNow.AddDays(-30);
        public bool IsOnSale => Price < 10;
        public new DateTime CreatedAt { get; set; }

    }

    
    public class MenuItemCreateViewModel : BaseCreateViewModel
    {
        [Required(ErrorMessage = "Item name is required")]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters")]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Required(ErrorMessage = "Image is required")]
        [Display(Name = "Image")]
        public IFormFile ImageFile { get; set; }

        [Display(Name = "Image URL")]
        [Url]
        public string ImageUrl { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; } = true;

        [Required(ErrorMessage = "Restaurant is required")]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }
        // restaurant name
        public string RestaurantName { get; set; } = string.Empty;

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        //PreparationTime
        [Display(Name = "Preparation Time")]
        public string PreparationTime { get; set; } = "40 - 60 minutes";

        // Categories and Restaurants
        public List<MenuItemCategoryViewModel> Categories { get; set; } = new();
    }

    /// <summary>
    /// View model for editing an existing menu item
    /// </summary>
    public class MenuItemEditViewModel : BaseEditViewModel
    {
        [Required(ErrorMessage = "Item name is required")]
        [StringLength(100, ErrorMessage = "Item name cannot exceed 100 characters")]
        [Display(Name = "Item Name")]
        public string Name { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
        [Display(Name = "Price")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Current Image")]
        [Url]
        public string CurrentImageUrl { get; set; }

        [Display(Name = "New Image")]
        public IFormFile? NewImageFile { get; set; }

        [Display(Name = "Available")]
        public bool IsAvailable { get; set; }

        [Required(ErrorMessage = "Restaurant is required")]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        [Display(Name = "Category")]
        public int? CategoryId { get; set; }

        // Categories and Restaurants
        public List<ResetaurantViewModel> Resetaurants { get; set; } = new();
        public List<MenuItemCategoryViewModel> Categories { get; set; } = new();
    }

    public class ResetaurantViewModel
    {
    }

    /// <summary>
    /// View model for displaying a list of menu items with pagination and filtering
    /// </summary>
    public class MenuItemListViewModel : BaseListViewModel<MenuItemViewModel>
    {
        public List<MenuItemViewModel> MenuItems 
        { 
            get => Items; 
            set => Items = value; 
        }

        public List<MenuItemCategoryViewModel> Categories { get; set; } = new();
        public List<RestaurantViewModel> Restaurants { get; set; } = new();
        public int? SelectedCategoryId { get; set; }
        public int? SelectedRestaurantId { get; set; }
        public string SearchQuery { get; set; } = string.Empty;
        public new string SortBy { get; set; } = "Name";
        public new string SortOrder { get; set; } = "asc";
        public int PageNumber { get; set; } = 1;
        public new int PageSize { get; set; } = 12;
        public int TotalItems { get; set; }
        public new int TotalPages => (int)Math.Ceiling((double)TotalItems / PageSize);
        public new bool HasPreviousPage => PageNumber > 1;
        public new bool HasNextPage => PageNumber < TotalPages;
    }

    public class MenuItemsByCategoryViewModel : BaseViewModel
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public string CategoryDescription { get; set; } = string.Empty;
        public List<MenuItemViewModel> MenuItems { get; set; } = new();
    }


    public class MenuItemDetailsViewModel : BaseViewModel
    {
        public string Name { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
        public bool IsAvailable { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<string> Allergens { get; set; } = new();
        public List<string> DietaryInfo { get; set; } = new();
        public decimal? Rating { get; set; }
        public int ReviewCount { get; set; }
        public int PreparationTime { get; set; }
        public bool IsSpicy { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }

        public bool IsPopular => Rating > 4.5m;
        // MenuItem
        public MenuItemViewModel MenuItem { get; set; } = new();
        public List<MenuItemViewModel> RelatedItems { get; set; } = new();
        public List<MenuItemReviewViewModel> Reviews { get; set; } = new();
    }
} 