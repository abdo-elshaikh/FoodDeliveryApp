using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.Payment;
using FoodDeliveryApp.ViewModels.OrderViewModels;
using Microsoft.AspNetCore.Http;

namespace FoodDeliveryApp.ViewModels.Profile
{
    public class ProfileViewModel
    {
        public string Id { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "First name is required")]
        [StringLength(50, ErrorMessage = "First name cannot exceed 50 characters")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Last name is required")]
        [StringLength(50, ErrorMessage = "Last name cannot exceed 50 characters")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Phone number is required")]
        [Phone(ErrorMessage = "Invalid phone number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = string.Empty;

        [Display(Name = "Profile Picture")]
        public string? ProfilePictureUrl { get; set; }
        
        [Display(Name = "Profile Picture Path")]
        public string? ProfilePicturePath { get; set; }

        public UserType UserType { get; set; }
        public string FullName => $"{FirstName} {LastName}";
    }

    public class EditProfileViewModel
    {
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

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        public string? CurrentProfilePictureUrl { get; set; }
    }

    public class CustomerProfileViewModel : ProfileViewModel
    {
        public int CustomerId { get; set; }
        
        [Required(ErrorMessage = "Date of birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }
        
        [Display(Name = "Receive Promotions")]
        public bool ReceivePromotions { get; set; }
        
        public int LoyaltyPoints { get; set; }

        //ProfilePicture
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        public List<AddressViewModel> Addresses { get; set; } = new();
        public List<PaymentMethodViewModel> PaymentMethods { get; set; } = new();
        public List<OrderHistoryViewModel> OrderHistory { get; set; } = new();
    }

    public class RestaurantProfileViewModel : ProfileViewModel
    {
        [Required(ErrorMessage = "Restaurant name is required")]
        [StringLength(100, ErrorMessage = "Restaurant name cannot exceed 100 characters")]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Cuisine type is required")]
        [Display(Name = "Cuisine Type")]
        public string CuisineType { get; set; } = string.Empty;

        [Display(Name = "Opening Hours")]
        public string OpeningHours { get; set; } = string.Empty;

        [Display(Name = "Delivery Fee")]
        [Range(0, 100, ErrorMessage = "Delivery fee must be between 0 and 100")]
        public decimal DeliveryFee { get; set; }

        [Display(Name = "Minimum Order Amount")]
        [Range(0, 1000, ErrorMessage = "Minimum order amount must be between 0 and 1000")]
        public decimal MinimumOrderAmount { get; set; }

        [Display(Name = "Average Delivery Time")]
        [Range(0, 120, ErrorMessage = "Average delivery time must be between 0 and 120 minutes")]
        public int AverageDeliveryTime { get; set; }

        public bool IsActive { get; set; }
        public double Rating { get; set; }
        public int TotalReviews { get; set; }
    }

    public class DeliveryDriverProfileViewModel : ProfileViewModel
    {
        [Required(ErrorMessage = "Vehicle type is required")]
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; } = string.Empty;

        [Required(ErrorMessage = "License plate is required")]
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
        public double Rating { get; set; }
        public int TotalDeliveries { get; set; }
        public List<DeliveryHistoryViewModel> DeliveryHistory { get; set; } = new();
    }

    public class EmployeeProfileViewModel : ProfileViewModel
    {
        public int EmployeeId { get; set; }
        
        [Required(ErrorMessage = "Position is required")]
        [StringLength(100, ErrorMessage = "Position cannot exceed 100 characters")]
        public EmployeePosition Position { get; set; } = EmployeePosition.CustomerService;
        
        [Required(ErrorMessage = "Hire date is required")]
        [Display(Name = "Hire Date")]
        public DateTime HireDate { get; set; }
        
        [Display(Name = "Termination Date")]
        public DateTime? TerminationDate { get; set; }
        
        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }
        
        public string? ProfilePicturePath { get; set; }
    }

    
} 