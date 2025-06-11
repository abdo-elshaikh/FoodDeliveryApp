using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FirstName { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string LastName { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? LastLogin { get; set; }

        public bool IsActive { get; set; } = true;

        [Required]
        public UserType Role { get; set; } = UserType.Customer;

        [StringLength(500)]
        public string? Notes { get; set; }

        // Navigation properties
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public virtual ICollection<Restaurant> Restaurants { get; set; } = new HashSet<Restaurant>();
        public virtual Driver? Driver { get; set; }

        // Helper properties
        public string FullName => $"{FirstName} {LastName}";

        [PersonalData]
        public DateTime? DateOfBirth { get; set; }

        [MaxLength(255)]
        public string ProfilePictureUrl { get; set; } = string.Empty;

        public override bool EmailConfirmed { get; set; } = false;

        public override bool PhoneNumberConfirmed { get; set; } = false;

        // list of Reviews
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();

        // Helper method to check if user has a specific role
        public bool HasRole(UserType role)
        {
            return Role == role;
        }
    }

    public enum UserType
    {
        Customer = 0,
        Driver = 1,
        Admin = 2,
        Owner = 3
    }
}
