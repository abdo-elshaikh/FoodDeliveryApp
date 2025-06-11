using FoodDeliveryApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class Review : BaseEntity
    {
        public int? RestaurantId { get; set; }
        public int? MenuItemId { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; } = null!;
        
        [Required]
        [Range(0, 5)]
        public decimal Rating { get; set; } = 1;

        [StringLength(1000)]
        public string? Content { get; set; }

        public string Title { get; set; } = string.Empty;


        // Navigation properties
        public virtual Restaurant? Restaurant { get; set; } = null!;
        public virtual MenuItem? MenuItem { get; set; }
        public virtual ApplicationUser User { get; set; } = null!;
    }
}