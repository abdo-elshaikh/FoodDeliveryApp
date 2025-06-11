using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.Order;
using FoodDeliveryApp.ViewModels.Restaurant;
using FoodDeliveryApp.ViewModels.Review;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Account
{
    public class UserProfileViewModel
    {
        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Please enter a valid email address")]
        [Display(Name = "Email Address")]
        public string Email { get; set; }

        [Display(Name = "First Name")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Please enter a valid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Profile Picture")]
        [DataType(DataType.Upload)]
        public IFormFile ProfilePicture { get; set; }

        public string ExistingProfilePicturePath { get; set; }

        [Display(Name = "Default Address")]
        public int? SelectedAddressId { get; set; }

        // Personal Information
        [Display(Name = "Full Name")]
        public string FullName => $"{FirstName} {LastName}".Trim();

        [Display(Name = "Account Created")]
        public DateTime? AccountCreatedDate { get; set; }

        [Display(Name = "Last Login")]
        public DateTime? LastLoginDate { get; set; }

        // Statistics
        [Display(Name = "Total Orders")]
        public int TotalOrders => Orders?.Count ?? 0;

        [Display(Name = "Total Reviews")]
        public int TotalReviews => Reviews?.Count ?? 0;

        [Display(Name = "Total Addresses")]
        public int TotalAddresses => Addresses?.Count ?? 0;

        [Display(Name = "Total Restaurants")]
        public int TotalRestaurants => Restaurants?.Count ?? 0;

        // Collections
        [Display(Name = "Order History")]
        public List<OrderSummaryViewModel> Orders { get; set; } = new List<OrderSummaryViewModel>();
        
        [Display(Name = "Addresses")]
        public List<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();
        
        [Display(Name = "Favorite Restaurants")]
        public List<RestaurantViewModel> Favorites { get; set; } = new List<RestaurantViewModel>();
        
        [Display(Name = "My Reviews")]
        public List<RestaurantReviewViewModel> Reviews { get; set; } = new List<RestaurantReviewViewModel>();
        
        [Display(Name = "My Restaurants")]
        public List<RestaurantViewModel> Restaurants { get; set; } = new List<RestaurantViewModel>();

        // Active tab tracking
        [Display(Name = "Active Tab")]
        public string ActiveTab { get; set; } = "profile-info";
    }
}
