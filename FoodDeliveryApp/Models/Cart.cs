using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace FoodDeliveryApp.Models
{
    public class Cart : BaseEntity
    {
        [Required]
        [ForeignKey(nameof(User))]
        public string UserId { get; set; }

        [Required]
        public CartStatus Status { get; set; } = CartStatus.Active;

        public new DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime LastModifiedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public DateTime ExpiresAt { get; set; } = DateTime.UtcNow.AddHours(24);

        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; } = 0;

        [Column(TypeName = "decimal(18,2)")]
        public decimal TaxRate { get; set; } = 0;

        // PromotionApplied
        [StringLength(50)]
        public string? PromotionCode { get; set; }
        public bool IsPromotionApplied { get; set; } = false;
        [DataType(DataType.Date)]
        public DateTime? PromotionCodeExpiration { get; set; }
        //PromotionDiscountAmount
        [Column(TypeName = "decimal(18,2)")]
        public decimal PromotionDiscountAmount { get; set; } = 0;
        //TotalWithDiscount
        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalWithDiscount { get; set; } = 0;

        [StringLength(500)]
        public string? SpecialInstructions { get; set; }

        public virtual ApplicationUser User { get; set; }

        public virtual ICollection<CartItem> Items { get; set; } = new HashSet<CartItem>();

        public bool IsEmpty => !Items.Any();

        public bool IsExpired => DateTime.UtcNow > ExpiresAt;

        public bool CanCheckout => !IsEmpty && !IsExpired && Status == CartStatus.Active;

        public decimal Subtotal => Items.Sum(item => item.Subtotal);

        public decimal Tax => Subtotal * (TaxRate / 100);

        public decimal Total => Subtotal + Tax + DeliveryFee - PromotionDiscountAmount;

        public bool HasMultipleRestaurants => Items.Select(i => i.RestaurantId).Distinct().Count() > 1;

        public Dictionary<int, decimal> RestaurantSubtotals => Items
            .GroupBy(i => i.RestaurantId)
            .ToDictionary(g => g.Key, g => g.Sum(i => i.Subtotal));

        public void AddItem(CartItem item)
        {
            if (IsExpired)
                throw new InvalidOperationException("Cannot add items to an expired cart");

            if (Status != CartStatus.Active)
                throw new InvalidOperationException("Cannot add items to a non-active cart");

            var existingItem = Items.FirstOrDefault(i =>
                i.MenuItemId == item.MenuItemId &&
                i.SpecialInstructions == item.SpecialInstructions
                );

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                Items.Add(item);
            }

            LastModifiedAt = DateTime.UtcNow;
        }

        public void RemoveItem(int itemId)
        {
            if (IsExpired)
                throw new InvalidOperationException("Cannot remove items from an expired cart");

            if (Status != CartStatus.Active)
                throw new InvalidOperationException("Cannot remove items from a non-active cart");

            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                Items.Remove(item);
                LastModifiedAt = DateTime.UtcNow;
            }
        }

        public void UpdateItemQuantity(int itemId, int quantity)
        {
            if (IsExpired)
                throw new InvalidOperationException("Cannot update items in an expired cart");

            if (Status != CartStatus.Active)
                throw new InvalidOperationException("Cannot update items in a non-active cart");

            var item = Items.FirstOrDefault(i => i.Id == itemId);
            if (item != null)
            {
                item.Quantity = quantity;
                LastModifiedAt = DateTime.UtcNow;
            }
        }

        public void Clear()
        {
            Items.Clear();
            PromotionCode = null;
            IsPromotionApplied = false;
            PromotionCodeExpiration = null;
            PromotionDiscountAmount = 0;
            TotalWithDiscount = 0;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void ApplyPromotion(string code, decimal discountAmount, DateTime expiration)
        {
            if (IsExpired)
                throw new InvalidOperationException("Cannot apply promotion to an expired cart");

            if (Status != CartStatus.Active)
                throw new InvalidOperationException("Cannot apply promotion to a non-active cart");

            PromotionCode = code;
            IsPromotionApplied = true;
            PromotionCodeExpiration = expiration;
            PromotionDiscountAmount = discountAmount;
            TotalWithDiscount = Total - discountAmount;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void RemovePromotion()
        {
            if (IsExpired)
                throw new InvalidOperationException("Cannot remove promotion from an expired cart");

            if (Status != CartStatus.Active)
                throw new InvalidOperationException("Cannot remove promotion from a non-active cart");

            PromotionCode = null;
            IsPromotionApplied = false;
            PromotionCodeExpiration = null;
            PromotionDiscountAmount = 0;
            TotalWithDiscount = Total;
            LastModifiedAt = DateTime.UtcNow;
        }

        public void ExtendExpiration(TimeSpan duration)
        {
            ExpiresAt = DateTime.UtcNow.Add(duration);
            LastModifiedAt = DateTime.UtcNow;
        }
    }

    public enum CartStatus
    {
        Active,
        CheckedOut,
        Abandoned,
        Expired
    }
}
