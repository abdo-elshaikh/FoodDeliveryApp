using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.AdminViewModels
{
    public class UserViewModel
    {
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
        
        // Additional properties for UI
        public string FullName => $"{FirstName} {LastName}";
        public string StatusBadge => IsActive ? 
            "<span class='badge bg-success'>Active</span>" : 
            "<span class='badge bg-danger'>Inactive</span>";
        public string RoleBadge => GetRoleBadge(Role);
        
        private static string GetRoleBadge(UserType role)
        {
            return role switch
            {
                UserType.Admin => "<span class='badge bg-danger'>Admin</span>",
                UserType.Customer => "<span class='badge bg-primary'>Customer</span>",
                UserType.Employee => "<span class='badge bg-secondary'>Employee</span>",
                UserType.Owner => "<span class='badge bg-warning'>Owner</span>",
                _ => "<span class='badge bg-dark'>Unknown</span>"
            };
        }
    }
} 