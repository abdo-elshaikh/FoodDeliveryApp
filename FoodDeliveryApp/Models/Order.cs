using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public enum OrderStatus
    {
        Placed,
        Confirmed,
        InPreparation,
        ReadyForPickup,
        OutForDelivery,
        Delivered,
        Canceled
    }

    

    public class Order
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Placed;

        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; }

        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [ForeignKey("Address")]
        public int? DeliveryAddressId { get; set; }

        [StringLength(500)]
        public string? SpecialInstructions { get; set; }

        [Required]
        public PaymentMethodType PaymentMethodType { get; set; }

        [StringLength(100)]
        public string? PaymentDetails { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", NullDisplayText = "Not delivered")]
        public DateTime? DeliveryDate { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", NullDisplayText = "Not available")]
        public DateTime? EstimatedDeliveryTime { get; set; }

        [StringLength(500)]
        public string? DeliveryInstructions { get; set; }

        [Url]
        [StringLength(200)]
        public string? TrackingUrl { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual Address? Address { get; set; }
        public virtual Payment? Payment { get; set; }
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
    }

    public class OrderItem
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("MenuItem")]
        public int MenuItemId { get; set; }

        [Required]
        [ForeignKey("Restaurant")]
        public int RestaurantId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(500)]
        public string? SpecialInstructions { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual MenuItem MenuItem { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual ICollection<OrderCustomization> Customizations { get; set; } = new HashSet<OrderCustomization>();
    }

    public class OrderCustomization
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [ForeignKey("OrderItem")]
        public int OrderItemId { get; set; }

        [Required]
        [ForeignKey("CustomizationOption")]
        public int OptionId { get; set; }

        [Required]
        [ForeignKey("CustomizationChoice")]
        public int ChoiceId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        // Navigation properties
        public virtual OrderItem OrderItem { get; set; } = null!;
        public virtual CustomizationOption CustomizationOption { get; set; } = null!;
        public virtual CustomizationChoice CustomizationChoice { get; set; } = null!;
    }

}