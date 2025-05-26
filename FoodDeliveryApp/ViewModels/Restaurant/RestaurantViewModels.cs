using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.Category;
using FoodDeliveryApp.ViewModels.MenuItem;
using FoodDeliveryApp.ViewModels.Review;
using FoodDeliveryApp.ViewModels.Promotion;
using Microsoft.AspNetCore.Http;

namespace FoodDeliveryApp.ViewModels.Restaurant
{
    public class RestaurantViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cuisine type is required")]
        [Display(Name = "Cuisine Type")]
        public string CuisineType { get; set; } = string.Empty;

        [Required(ErrorMessage = "Address is required")]
        public AddressViewModel Address { get; set; } = new();

        
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Display(Name = "Website")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? Website { get; set; }

        [Display(Name = "Logo URL")]
        public string? LogoUrl { get; set; }

        [Display(Name = "Cover Image URL")]
        public string? CoverImageUrl { get; set; }

        [Display(Name = "Delivery Fee")]
        [Range(0, 100, ErrorMessage = "Delivery fee must be between 0 and 100")]
        public decimal DeliveryFee { get; set; }

        [Display(Name = "Minimum Order Amount")]
        [Range(0, 1000, ErrorMessage = "Minimum order amount must be between 0 and 1000")]
        public decimal MinimumOrderAmount { get; set; }

        [Display(Name = "Average Delivery Time")]
        [Range(0, 120, ErrorMessage = "Average delivery time must be between 0 and 120 minutes")]
        public int AverageDeliveryTime { get; set; }

        // tax rate
        [Display(Name = "Tax Rate (%)")]
        [Range(0, 30, ErrorMessage = "Tax rate must be between 0 and 30%")]
        public decimal TaxRate { get; set; }
        public bool IsActive { get; set; }
        public bool IsOpen { get; set; }
        public double Rating { get; set; }
        public int TotalReviews { get; set; }
        public int TotalMenuItems { get; set; }

        //CategoryName
        public string? CategoryName { get; set; }

        // opening hours
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        // location
        public string LocationUrl { get; set; } = string.Empty;
        // City


        //IsAdminOrOwner
        public bool IsAdminOrOwner { get; set; } = false;
        public List<CategoryViewModel> Categories { get; set; } = new();
        public List<MenuItemViewModel> MenuItems { get; set; } = new();
    }

    public class CreateRestaurantViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Category")]
        [Required(ErrorMessage = "Category is required")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City cannot exceed 50 characters")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Opening Time")]
        public TimeSpan OpeningTime { get; set; }

        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }

        [Display(Name = "Website")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? Website { get; set; }
        
        [Display(Name = "Map Location URL")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? LocationUrl { get; set; }

        [Display(Name = "Restaurant Image")]
        public IFormFile? ImageFile { get; set; }

        [Display(Name = "Image URL")]
        [Url(ErrorMessage = "Invalid URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Delivery Fee")]
        [Range(0, 100, ErrorMessage = "Delivery fee must be between 0 and 100")]
        public decimal DeliveryFee { get; set; }

        [Display(Name = "Tax Rate (%)")]
        [Range(0, 30, ErrorMessage = "Tax rate must be between 0 and 30%")]
        public decimal TaxRate { get; set; }

        [Display(Name = "Owner")]
        public string? OwnerId { get; set; }

        public bool IsActive { get; set; } = true;
        
        // Navigation properties
        [Display(Name = "Restaurant Category")]
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList? Categories { get; set; }
        
        [Display(Name = "Restaurant Owner")]
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList? Owners { get; set; }
    }

    public class EditRestaurantViewModel : CreateRestaurantViewModel
    {
        public int Id { get; set; }
        public string? CurrentLogoUrl { get; set; }
        public string? CurrentCoverImageUrl { get; set; }
    }

    public class RestaurantListViewModel
    {
        public IEnumerable<RestaurantViewModel> Restaurants { get; set; } = new List<RestaurantViewModel>();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 9;
        public int TotalPages { get; set; }
        public string? SearchTerm { get; set; }
        public string? CurrentSortOrder { get; set; }
        public int? CurrentCategoryId { get; set; }
        public bool IsAdmin { get; set; }
        public string? MaxDeliveryFee { get; set; }
        public List<string> SelectedCuisines { get; set; } = new();
        public List<string> SelectedDietaryOptions { get; set; } = new();
        public Microsoft.AspNetCore.Mvc.Rendering.SelectList? Categories { get; set; }
    }

    public class RestaurantSearchViewModel
    {
        public string? SearchTerm { get; set; }
        public string? CuisineType { get; set; }
        public double? MinRating { get; set; }
        public decimal? MaxDeliveryFee { get; set; }
        public bool? IsOpen { get; set; }
        public string? SortBy { get; set; }
        public bool SortDescending { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class RestaurantDetailViewModel
    {
        public RestaurantViewModel Restaurant { get; set; } = new();
        public List<RestaurantMenuItemViewModel> MenuItems { get; set; } = new();
        public List<ReviewViewModel> Reviews { get; set; } = new();
        public List<PromotionViewModel> Promotions { get; set; } = new();
        public bool IsAdminOrOwner { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsOwner { get; set; }
        
    }

    public class RestaurantMenuItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string? ImageUrl { get; set; }
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
        public string CategoryName { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public int ReviewCount { get; set; }
    }
} 