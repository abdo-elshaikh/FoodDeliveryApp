using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models

{
    public class CartItem : BaseEntity
    {

        [Required]
        public int CartId { get; set; }

        [Required]
        public int MenuItemId { get; set; }

        public int RestaurantId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        public string? SpecialInstructions { get; set; }
        // modefires
        

        [NotMapped]
        public decimal Subtotal => MenuItem?.Price * Quantity ?? 0;
        
        // Navigation properties
        public virtual Cart Cart { get; set; } = null!;
        public virtual MenuItem MenuItem { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
    }
}
