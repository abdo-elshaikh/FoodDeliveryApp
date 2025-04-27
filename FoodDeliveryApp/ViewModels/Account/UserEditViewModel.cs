using FoodDeliveryApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Account
{
    public class UserEditViewModel
    {
        [Required]
        public string Id { get; set; }

        [Required(ErrorMessage = "Email is required")]
        [EmailAddress(ErrorMessage = "Invalid email address")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [StringLength(50, ErrorMessage = "First name cannot be longer than 50 characters")]
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [StringLength(50, ErrorMessage = "Last name cannot be longer than 50 characters")]
        [Display(Name = "Last Name")]
        public string? LastName { get; set; }
        public bool IsActive { get; set; }

        public bool IsEmailConfirmed { get; set; }

        [Display(Name = "Role")]
        public UserRole Role { get; set; }

    }
}