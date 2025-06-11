using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.Order;
using FoodDeliveryApp.ViewModels.Review;
using FoodDeliveryApp.ViewModels.Promotion;
using FoodDeliveryApp.ViewModels.Dashboard;
using Microsoft.AspNetCore.Http;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Account;

namespace FoodDeliveryApp.ViewModels.Admin
{
    public class AdminDashboardViewModel
    {
        [Display(Name = "Total Users")]
        public int TotalUsers { get; set; }

        [Display(Name = "Total Restaurants")]
        public int TotalRestaurants { get; set; }

        [Display(Name = "Total Orders")]
        public int TotalOrders { get; set; }

        [Display(Name = "Total Revenue")]
        [DataType(DataType.Currency)]
        public decimal TotalRevenue { get; set; }

        
        [Display(Name = "Active Promotions")]
        public int ActivePromotions { get; set; }

        
        [Display(Name = "Pending Approvals")]
        public int PendingApprovals { get; set; }

        
        [Display(Name = "Total Reviews")]
        public int TotalReviews { get; set; }
        
        
        public List<RestaurantListViewModel> NewRestaurants { get; set; } = new();
        
        
        public List<UserSummaryViewModel> NewUsers { get; set; } = new();
        
        
        public List<OrderSummaryViewModel> RecentOrders { get; set; } = new();
        
        
        public List<RestaurantReviewViewModel> RecentReviews { get; set; } = new();
                
        public List<PromotionViewModel> ActivePromotionsList { get; set; } = new();
    }
        
    public class UserManagementViewModel
    {
        
        public List<UserSummaryViewModel> Users { get; set; } = new();

        
        public int TotalCount { get; set; }

        
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int CurrentPage { get; set; } = 1;

        
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? SearchTerm { get; set; }

        
        [StringLength(50, ErrorMessage = "Sort field cannot exceed 50 characters")]
        public string? SortBy { get; set; }

        
        [StringLength(4, ErrorMessage = "Sort order cannot exceed 4 characters")]
        public string? SortOrder { get; set; }
    }
        
    public class UserSummaryViewModel
    {
        
        [Required(ErrorMessage = "User ID is required")]
        public string Id { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "User type is required")]
        public string UserType { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Registration date is required")]
        [DataType(DataType.Date)]
        public DateTime RegistrationDate { get; set; }

        
        public bool IsActive { get; set; }

        
        [Url(ErrorMessage = "Invalid URL format")]
        public string? ProfilePictureUrl { get; set; }
    }
        
    public class RestaurantManagementViewModel
    {
        
        public List<RestaurantListViewModel> Restaurants { get; set; } = new();

        
        public int TotalCount { get; set; }

        
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int CurrentPage { get; set; } = 1;

        
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? SearchTerm { get; set; }

        
        [StringLength(50, ErrorMessage = "Cuisine type cannot exceed 50 characters")]
        public string? CuisineType { get; set; }

        
        [StringLength(50, ErrorMessage = "Sort field cannot exceed 50 characters")]
        public string? SortBy { get; set; }

        
        [StringLength(4, ErrorMessage = "Sort order cannot exceed 4 characters")]
        public string? SortOrder { get; set; }
    }
        
    public class OrderManagementViewModel
    {
        
        public List<OrderSummaryViewModel> Orders { get; set; } = new();

        
        public int TotalCount { get; set; }

        
        [Range(1, int.MaxValue, ErrorMessage = "Page number must be greater than 0")]
        public int CurrentPage { get; set; } = 1;

        
        [Range(1, 100, ErrorMessage = "Page size must be between 1 and 100")]
        public int PageSize { get; set; } = 10;

        
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        
        [StringLength(100, ErrorMessage = "Search term cannot exceed 100 characters")]
        public string? SearchTerm { get; set; }

        
        [StringLength(20, ErrorMessage = "Status cannot exceed 20 characters")]
        public string? Status { get; set; }

        
        [StringLength(50, ErrorMessage = "Sort field cannot exceed 50 characters")]
        public string? SortBy { get; set; }

        
        [StringLength(4, ErrorMessage = "Sort order cannot exceed 4 characters")]
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
        
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = string.Empty;
        
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
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
        
        
        public string FullName => $"{FirstName} {LastName}";
        
        
        public string StatusBadge => IsActive ? 
            "<span class='badge bg-success'>Active</span>" : 
            "<span class='badge bg-danger'>Inactive</span>";
        
        
        public string RoleBadge => GetRoleBadge(Role);
        
        
        /// <param name="role">The user role to get the badge for</param>
        /// <returns>HTML badge string for the role</returns>
        private static string GetRoleBadge(UserType role)
        {
            return role switch
            {
                UserType.Admin => "<span class='badge bg-danger'>Admin</span>",
                UserType.Customer => "<span class='badge bg-primary'>Customer</span>",
                UserType.Driver => "<span class='badge bg-info'>Driver</span>",
                UserType.Owner => "<span class='badge bg-warning'>Owner</span>",
                _ => "<span class='badge bg-dark'>Unknown</span>"
            };
        }
    }

    
    public class CreateUserViewModel
    {
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
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
        
        
        [Required(ErrorMessage = "Password is required")]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;
        
        
        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }

    
    public class EditUserViewModel
    {
        
        [Required(ErrorMessage = "User ID is required")]
        public string UserId { get; set; } = string.Empty;
        
        
        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
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

    
    public class AdminViewModel
    {
        
        public List<UserViewModel> Users { get; set; } = new();
        
        
        public CreateUserViewModel CreateUser { get; set; } = new();
        
        
        public EditUserViewModel EditUser { get; set; } = new();
        
        
        public int CurrentPage { get; set; } = 1;
        
        
        public int TotalPages { get; set; }
        
        
        public string SearchTerm { get; set; } = string.Empty;
        
        
        public UserType? RoleFilter { get; set; }
        
        
        public bool ShowActiveOnly { get; set; } = true;
    }
} 