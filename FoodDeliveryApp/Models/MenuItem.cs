using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class MenuItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        // Make CategoryId nullable to support ON DELETE SET NULL
        public int? CategoryId { get; set; }
        public virtual MenuItemCategory Category { get; set; }

        // Navigation properties
        public virtual Restaurant Restaurant { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<CustomizationOption> CustomizationOptions { get; set; } = new HashSet<CustomizationOption>();
    }

    public class CustomizationOption
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public bool IsRequired { get; set; }
        public bool AllowMultiple { get; set; }
        public List<CustomizationChoice> Choices { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }

    public class CustomizationChoice
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(CustomizationOption))]
        public int CustomizationOptionId { get; set; }

        public string Name { get; set; }
        public decimal Price { get; set; }
        public bool IsDefault { get; set; }

        public virtual CustomizationOption CustomizationOption { get; set; }
    }

}
