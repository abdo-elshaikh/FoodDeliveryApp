using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.OrderViewModels;
using FoodDeliveryApp.ViewModels.Review;
using FoodDeliveryApp.ViewModels.Promotion;
using Microsoft.AspNetCore.Http;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        public int TotalUsers { get; set; }
        public int TotalRestaurants { get; set; }
        public int TotalOrders { get; set; }
        public decimal TotalRevenue { get; set; }
        public int ActivePromotions { get; set; }
        public int PendingApprovals { get; set; }
        public int TotalReviews { get; set; }
        
        public List<RestaurantListViewModel> NewRestaurants { get; set; } = new();
        public List<UserSummaryViewModel> NewUsers { get; set; } = new();
        public List<OrderSummaryViewModel> RecentOrders { get; set; } = new();
        public List<ReviewListViewModel> RecentReviews { get; set; } = new();
        public List<PromotionViewModel> ActivePromotionsList { get; set; } = new();
    }
    
    public class UserManagementViewModel
    {
        public List<UserSummaryViewModel> Users { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
    
    public class UserSummaryViewModel
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string UserType { get; set; } = string.Empty;
        public DateTime RegistrationDate { get; set; }
        public bool IsActive { get; set; }
        public string? ProfilePictureUrl { get; set; }
    }
    
    public class RestaurantManagementViewModel
    {
        public List<RestaurantListViewModel> Restaurants { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SearchTerm { get; set; }
        public string? CuisineType { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
    
    public class OrderManagementViewModel
    {
        public List<OrderSummaryViewModel> Orders { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SearchTerm { get; set; }
        public string? Status { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
    
    public class AdminSettingsViewModel
    {
        [Required(ErrorMessage = "Site name is required")]
        [StringLength(100, ErrorMessage = "Site name cannot exceed 100 characters")]
        [Display(Name = "Site Name")]
        public string SiteName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Contact email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Contact Email")]
        public string ContactEmail { get; set; } = string.Empty;
        
        [Display(Name = "Logo")]
        public IFormFile? Logo { get; set; }
        
        [Display(Name = "Current Logo")]
        public string? CurrentLogoUrl { get; set; }
        
        [Required(ErrorMessage = "Default tax rate is required")]
        [Range(0, 30, ErrorMessage = "Tax rate must be between 0% and 30%")]
        [Display(Name = "Default Tax Rate (%)")]
        public decimal DefaultTaxRate { get; set; }
        
        [Required(ErrorMessage = "Default service fee is required")]
        [Range(0, 20, ErrorMessage = "Service fee must be between 0% and 20%")]
        [Display(Name = "Default Service Fee (%)")]
        public decimal DefaultServiceFee { get; set; }
        
        [Required(ErrorMessage = "Minimum order amount is required")]
        [Range(0, 100, ErrorMessage = "Minimum order amount must be between $0 and $100")]
        [Display(Name = "Minimum Order Amount ($)")]
        public decimal MinimumOrderAmount { get; set; }
        
        [Display(Name = "Enable User Registration")]
        public bool EnableUserRegistration { get; set; } = true;
        
        [Display(Name = "Enable Restaurant Registration")]
        public bool EnableRestaurantRegistration { get; set; } = true;
        
        [Display(Name = "Enable Driver Registration")]
        public bool EnableDriverRegistration { get; set; } = true;
        
        [Display(Name = "Enable Reviews")]
        public bool EnableReviews { get; set; } = true;
        
        [Display(Name = "Require Email Verification")]
        public bool RequireEmailVerification { get; set; } = true;
    }
    
    public class UserViewModel
    {
        public string UserId { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "User role is required")]
        [Display(Name = "Role")]
        public UserType Role { get; set; }
        
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
    }
} 