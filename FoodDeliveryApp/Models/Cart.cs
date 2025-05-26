using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        public virtual ApplicationUser User { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        public List<CartItem> Items { get; set; } = new List<CartItem>();

        [NotMapped]
        public decimal TotalPrice => Items.Sum(item => item.Subtotal);
        [NotMapped]
        public int TotalItems => Items.Sum(item => item.Quantity);
        [NotMapped]
        public bool IsEmpty => !Items.Any();
        [NotMapped]
        public Dictionary<string, decimal> RestaurantTotals =>
            Items.GroupBy(i => i.MenuItem.Restaurant.Name)
                .ToDictionary(g => g.Key, g => g.Sum(i => i.Subtotal));

    }

    public class CartItem
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public Cart Cart { get; set; }

        [Required]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        public MenuItem MenuItem { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }
        public List<Customization> Customizations { get; set; } = new List<Customization>();

        [NotMapped]
        public decimal Subtotal => MenuItem?.Price * Quantity ?? 0;
    }

    public class Customization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int CartItemId { get; set; }

        [ForeignKey("CartItemId")]
        public CartItem CartItem { get; set; }

        [Required]
        public int OptionId { get; set; }
        [ForeignKey("OptionId")]
        public CustomizationOption Option { get; set; }

        [Required]
        public int ChoiceId { get; set; }
        [ForeignKey("ChoiceId")]
        public CustomizationChoice Choice { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }
    }
}
