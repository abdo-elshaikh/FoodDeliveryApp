using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public enum OrderStatus
    {
        Pending,
        Processing,
        Shipped,
        Delivered,
        Cancelled
    }

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        public int? DeliveryAddressId { get; set; }
        public string? SpecialInstructions { get; set; } = string.Empty;

        [Required]
        public PaymentMethodType PaymentMethodType { get; set; }

        [StringLength(100)]
        public string? PaymentDetails { get; set; }

        // Navigation properties
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public virtual ApplicationUser User { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual Restaurant? Restaurant { get; set; }
        public virtual Address? Address { get; set; }
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int OrderId { get; set; }

        [Required]
        public int MenuItemId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public string SpecialInstructions { get; set; } = string.Empty;

        // Navigation properties
        public List<OrderCustomization> Customizations { get; set; } = new List<OrderCustomization>();
        public virtual Order Order { get; set; }
        public virtual MenuItem MenuItem { get; set; }
        public virtual Restaurant Restaurant { get; set; }
    }

    public class OrderCustomization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int OrderItemId { get; set; }

        public OrderItem OrderItem { get; set; } = new OrderItem();

        [Required]
        public int OptionId { get; set; }

        [Required]
        public int ChoiceId { get; set; }

        [Required]
        public decimal Price { get; set; }
    }
}