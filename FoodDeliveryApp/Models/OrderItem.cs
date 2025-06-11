using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace FoodDeliveryApp.Models
{
    public class OrderItem : BaseEntity
    {
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
        public int Quantity { get; set; } = 1;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; } = 0;

        [StringLength(500)]
        public string? SpecialInstructions { get; set; } = string.Empty;

        // Modifiers
        [Column(TypeName = "nvarchar(max)")]
        public string Modifiers { get; set; } = "[]";

        public bool IsPrepared { get; set; }

        public OrderItemStatus Status { get; set; } = OrderItemStatus.Pending;

        public DateTime? PreparedAt { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        public virtual MenuItem MenuItem { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
    }

    public enum OrderItemStatus
    {
        Pending,
        Confirmed,
        Preparing,
        Ready,
        Delivered,
        Canceled
    }
} 