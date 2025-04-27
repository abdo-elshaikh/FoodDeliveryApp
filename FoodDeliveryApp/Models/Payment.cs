using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public enum PaymentMethodType
    {
        CreditCard,
        DebitCard,
        PayPal,
        ApplePay,
        GooglePay,
        CashOnDelivery
    }

    public enum PaymentStatus
    {
        Pending,
        Completed,
        Failed,
        Refunded
    }
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        public PaymentMethodType Type { get; set; }

        [Required]
        [StringLength(100)]
        public string Provider { get; set; }

        [StringLength(100)]
        public string? AccountNumberMasked { get; set; }
        public bool IsDefault { get; set; } = false;
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
        public int OrderId { get; set; }

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
        public virtual Order Order { get; set; }
        public virtual PaymentMethod? PaymentMethod { get; set; }
    }

    public class Promotion
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; }

        [Required]
        public bool IsPercentage { get; set; } = false;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        public int? RestaurantId { get; set; } // Null means site-wide promotion

        public int? UsageLimit { get; set; }

        public int? MinimumOrderAmount { get; set; }

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
        public string UserId { get; set; }
        [Required]
        public int OrderId { get; set; }
        [Required]
        public int PromotionId { get; set; }
        [Required]
        public DateTime UsedAt { get; set; } = DateTime.UtcNow;
        [ForeignKey("PromotionId")]
        public virtual Promotion Promotion { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order Order { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
    }

    public class Review
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        public int CustomerProfileId { get; set; }

        [Required]
        [Range(0, 5)]
        public decimal Rating { get; set; }

        [StringLength(1000)]
        public string? Comment { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        // Navigation properties
        public virtual Restaurant Restaurant { get; set; }
        public virtual CustomerProfile CustomerProfile { get; set; }
    }
}