using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Profile
{
    public class CustomerProfileViewModel
    {
        public int CustomerId { get; set; }
        [Required(ErrorMessage = "First name is required")]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required")]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Email address is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Phone number is required")]
        [Display(Name = "Phone Number")]
        [Phone(ErrorMessage = "Invalid phone number")]
        public string PhoneNumber { get; set; }

        [Required(ErrorMessage = "Date of birth is required")]
        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Display(Name = "Profile Picture")]
        public IFormFile? ProfilePicture { get; set; }

        // For display only
        public string? ProfilePicturePath { get; set; }

        public decimal? LoyaltyPoints { get; set; }
        public bool ReceivePromotions { get; set; } = false;

        // Full name property for display purposes
        public string FullName => $"{FirstName} {LastName}";
    }
}