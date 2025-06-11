using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Promotion : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(1000)]
        public string Description { get; set; } = string.Empty;

        [StringLength(255)]
        public string? ImageUrl { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string Code { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountAmount { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal MinimumOrderAmount { get; set; } = 0m;

        public bool IsPercentage { get; set; } = false;

        public DateTime ValidUntil { get; set; } = DateTime.UtcNow;

        public bool IsActive { get; set; } = true;

        // usige counter
        [Column(TypeName = "int")]
        public int UsageCount { get; set; } = 0;

        // max usage limit
        [Column(TypeName = "int")]
        public int? MaxUsageLimit { get; set; } = null;

        public int? RestaurantId { get; set; }
        public virtual Restaurant? Restaurant { get; set; }

        public DiscountType DiscountType { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal DiscountValue { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public int? UsageLimit { get; set; }

        // help methods
        public bool IsExpired()
        {
            return ValidUntil < DateTime.UtcNow;
        }

        public bool IsValid()
        {
            return !IsExpired() && IsActive;
        }

        // update usage count
        public void IncrementUsageCount()
        {
            UsageCount++;
        }
        
        // calculate discount amount
        public decimal CalculateDiscountAmount(decimal totalAmount)
        {
            if (IsPercentage)
            {
                return totalAmount * (DiscountAmount / 100);
            }
            return DiscountAmount;
        }
    }

    public enum DiscountType
    {
        Percentage,
        FixedAmount
    }
}
