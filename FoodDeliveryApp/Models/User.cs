using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
        public enum UserRole
        {
            Customer,
            Employee,
            Admin
        }
    public class User : IdentityUser
    {

        [Required, EmailAddress, StringLength(100)]
        [Display(Name = "Email Address")]
        public override string Email { get; set; }

        public UserRole Role { get; set; } = UserRole.Customer;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual Customer? Customer { get; set; }
        public virtual Employee? Employee { get; set; }
    }
}
