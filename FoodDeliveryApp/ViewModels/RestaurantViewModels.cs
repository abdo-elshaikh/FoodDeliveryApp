using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.RestaurantViewModels
{

    public class RestaurantViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string? ImageUrl { get; set; } = string.Empty;
        public decimal Rating { get; set; }
        public string CategoryName { get; set; }
        public TimeSpan OpeningTime { get; set; }
        public TimeSpan ClosingTime { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public bool IsOpen { get; set; }
        public bool IsActive { get; set; }
        public string? WebsiteUrl { get; set; } = string.Empty;
        public string? LocationUrl { get; set; } = string.Empty;
        public decimal DeliveryFee { get; set; } = 0;
        public decimal TaxRate { get; set; } = 0;
        //EstimatedDeliveryTime
        public int EstimatedDeliveryTime { get; set; } = 30; // in minutes
        public bool IsAdminOrOwner { get; set; } = false;

    }

    public class RestaurantListViewModel
    {
        public IEnumerable<RestaurantViewModel> Restaurants { get; set; }
        public SelectList Categories { get; set; }
        public int? CurrentCategoryId { get; set; }
        public string SearchTerm { get; set; }
        public string CurrentSortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool IsAdmin { get; set; } = false;
    }

    public class RestaurantDetailViewModel
    {
        public RestaurantViewModel Restaurant { get; set; }
        public IEnumerable<RestaurantMenuItemViewModel> MenuItems { get; set; }
        public IEnumerable<ReviewViewModel> Reviews { get; set; }
        public IEnumerable<PromotionViewModel> Promotions { get; set; }
        public bool IsAdminOrOwner { get; set; } = false;
    }

    public class PromotionViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal DiscountPercentage { get; set; }
        public DateTime ValidUntil { get; set; }
        public string PromoCode { get; set; }
    }

    public class MenuCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IEnumerable<RestaurantMenuItemViewModel> Items { get; set; }
    }

    public class RestaurantCreateViewModel
    {
        [Required(ErrorMessage = "Restaurant name is required")]
        [MaxLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Description is required")]
        [MaxLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Address is required")]
        [MaxLength(200, ErrorMessage = "Address cannot exceed 200 characters")]
        public string Address { get; set; }

        [Required(ErrorMessage = "City is required")]
        [MaxLength(100, ErrorMessage = "City cannot exceed 100 characters")]
        public string City { get; set; }

        [Required(ErrorMessage = "State is required")]
        [MaxLength(50, ErrorMessage = "State cannot exceed 50 characters")]
        public string State { get; set; }

        [Required(ErrorMessage = "Postal code is required")]
        [MaxLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required(ErrorMessage = "Opening time is required")]
        [Display(Name = "Opening Time")]
        public TimeSpan OpeningTime { get; set; }

        [Required(ErrorMessage = "Closing time is required")]
        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }
        [Display(Name = "Website URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? WebsiteUrl { get; set; } = string.Empty;

        [Display(Name = "Location URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? LocationUrl { get; set; } = string.Empty;
        [Display(Name = "Delivery Fee")]
        [Range(0, double.MaxValue, ErrorMessage = "Delivery fee must be a positive number")]
        public decimal DeliveryFee { get; set; } = 0;
        [Display(Name = "Tax Rate")]
        [Range(0, 1, ErrorMessage = "Tax rate must be between 0 and 1")]
        public decimal TaxRate { get; set; } = 0;

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public string OwnerId { get; set; }

        [Display(Name = "Restaurant Image")]
        public IFormFile? ImageFile { get; set; } = null;

        [Display(Name = "Image URL")]
        [Url(ErrorMessage = "Please enter a valid URL")]
        public string? ImageUrl { get; set; } = string.Empty;

        public IEnumerable<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
        public IEnumerable<SelectListItem> Owners { get; set; } = new List<SelectListItem>();
    }

    public class RestaurantEditViewModel : RestaurantCreateViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Active Status")]
        public bool IsActive { get; set; } = true;
    }

    public class RestaurantCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class RestaurantOwnerViewModel
    {
        public string Id { get; set; }
        public string FullName { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
    }

    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal Rating { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }
    }

    public class RestaurantMenuItemViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string ImageUrl { get; set; }
        public RestaurantViewModel Restaurant { get; set; }
    }

    public class PromotionApplyViewModel
    {
        public int RestaurantId { get; set; }
        public string PromoCode { get; set; }
        public decimal TotalAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public bool IsPromoApplied { get; set; } = false;
    }
}
