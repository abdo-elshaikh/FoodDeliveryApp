using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Restaurant
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Phone]
        public string PhoneNumber { get; set; }

        [Required]
        public int CategoryId { get; set; }

        [Required]
        [StringLength(255)]
        public string Address { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(20)]
        public string PostalCode { get; set; }

        [Required]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        public TimeSpan ClosingTime { get; set; }


        public bool IsActive { get; set; } = true;

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public decimal Rating { get; set; }
        public string? WebsiteUrl { get; set; }
        public string? LocationUrl { get; set; }
        // Delivery fee
        public decimal DeliveryFee { get; set; } = 0;
        // Tax rate
        public decimal TaxRate { get; set; } = 0;

        [Required]
        [ForeignKey(nameof(ApplicationUser))]
        public string OwnerId { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual RestaurantCategory Category { get; set; }
        public virtual ApplicationUser Owner { get; set; }
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new HashSet<MenuItem>();
        public virtual ICollection<Order> Orders { get; set; } = new HashSet<Order>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Promotion> Promotions { get; set; } = new HashSet<Promotion>();

        public int NumberOfReviews => Reviews.Count;
    }
}
