using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class CustomerProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public bool IsActive { get; set; } = true;

        [StringLength(255)]
        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool ReceivePromotions { get; set; } = false;

        [Column(TypeName = "decimal(18,2)")]
        public decimal LoyaltyPoints { get; set; } = 0;

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public virtual ICollection<Address> Addresses { get; set; } = new HashSet<Address>();
        public virtual ICollection<PaymentMethod> PaymentMethods { get; set; } = new HashSet<PaymentMethod>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }
}
