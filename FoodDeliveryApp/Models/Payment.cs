using FoodDeliveryApp.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed,
    Refunded
}

public enum PaymentMethodType
{
    CreditCard,
    DebitCard,
    PayPal,
    CashOnDelivery,
    Other
}

public class PaymentMethod
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    public PaymentMethodType Type { get; set; }

    [Required]
    [StringLength(100)]
    public string Provider { get; set; } = string.Empty;

    [StringLength(100)]
    public string? AccountNumberMasked { get; set; }

    public bool IsDefault { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual ApplicationUser User { get; set; }
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}

public class Payment
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Order")]
    public int OrderId { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [ForeignKey("PaymentMethod")]
    public int? PaymentMethodId { get; set; }

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Amount { get; set; }

    [Required]
    public PaymentStatus Status { get; set; }

    [Required]
    public DateTime PaymentDate { get; set; } = DateTime.UtcNow;

    [StringLength(255)]
    public string? TransactionId { get; set; }

    // Navigation properties
    public virtual Order Order { get; set; } = null!;
    public virtual PaymentMethod? PaymentMethod { get; set; }
    public virtual ApplicationUser User { get; set; } = null!;
}

public class Promotion
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Code { get; set; } = string.Empty;

    [Required]
    [StringLength(255)]
    public string Description { get; set; } = string.Empty;

    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal DiscountValue { get; set; }

    public bool IsPercentage { get; set; }

    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [ForeignKey("Restaurant")]
    public int? RestaurantId { get; set; }

    [Range(0, int.MaxValue)]
    public int? UsageLimit { get; set; }

    [Column(TypeName = "decimal(18,2)")]
    public decimal? MinimumOrderAmount { get; set; }

    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Restaurant? Restaurant { get; set; }
    public virtual ICollection<PromotionUsage> PromotionUsages { get; set; } = new HashSet<PromotionUsage>();
}

public class PromotionUsage
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string UserId { get; set; } = string.Empty;

    [Required]
    [ForeignKey("Order")]
    public int OrderId { get; set; }

    [Required]
    [ForeignKey("Promotion")]
    public int PromotionId { get; set; }

    [Required]
    public DateTime UsedAt { get; set; } = DateTime.UtcNow;

    // Navigation properties
    public virtual Promotion Promotion { get; set; } = null!;
    public virtual Order Order { get; set; } = null!;
    public virtual ApplicationUser User { get; set; } = null!;
}

public class Review
{
    [Key]
    public int Id { get; set; }

    [Required]
    [ForeignKey("Restaurant")]
    public int RestaurantId { get; set; }

    [Required]
    [ForeignKey("CustomerProfile")]
    public int CustomerProfileId { get; set; }

    [ForeignKey("MenuItem")]
    public int? MenuItemId { get; set; }

    [ForeignKey("Order")]
    public int? OrderId { get; set; }

    [Required]
    [Range(0, 5)]
    public decimal Rating { get; set; }

    [StringLength(1000)]
    public string? Comment { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? UpdatedAt { get; set; }

    // Navigation properties
    public virtual Restaurant Restaurant { get; set; } = null!;
    public virtual CustomerProfile CustomerProfile { get; set; } = null!;
    public virtual MenuItem? MenuItem { get; set; }
    public virtual Order? Order { get; set; }
}