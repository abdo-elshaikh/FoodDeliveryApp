﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public enum PaymentMethod
    {
        CashOnDelivery,
        VodafoneCash,
        Visa,
        MasterCard
    }

    public enum PaymentStatus
    {
        Pending,
        Processing,
        Completed,
        Failed,
        Refunded
    }

    public class Order : BaseEntity
    {
        [Required]
        [StringLength(50)]
        public string OrderNumber { get; set; } = string.Empty;

        [Required]
        [ForeignKey("User")]
        public string UserId { get; set; } = string.Empty;

        [ForeignKey("Driver")]
        public int? DriverId { get; set; }

        [Required]
        public int DeliveryAddressId { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public DateTime OrderDate { get; set; } = DateTime.UtcNow;

        public DateTime? ActualDeliveryTime { get; set; }

        public DateTime? EstimatedDeliveryTime { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Subtotal { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Tax { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal DeliveryFee { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Discount { get; set; } = 0;

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Total { get; set; } = 0;

        [Required]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.CashOnDelivery;

        [Required]
        public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

        [StringLength(500)]
        public string? Notes { get; set; } = string.Empty;

        [StringLength(255)]
        public string? TrackingUrl { get; set; }

        public bool IsDelayed { get; set; }

        [StringLength(200)]
        public string? DelayReason { get; set; }

        public bool RequiresSignature { get; set; }

        [StringLength(255)]
        public string? SignatureImageUrl { get; set; }

        [StringLength(255)]
        public string? DeliveryPhotos { get; set; }

        [NotMapped]
        [Column(TypeName = "jsonb")]
        public Dictionary<string, string> CustomFields { get; set; } = new Dictionary<string, string>();

        // Navigation properties
        public virtual ApplicationUser User { get; set; } = null!;
        public virtual Driver? Driver { get; set; }
        public virtual Address DeliveryAddress { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public virtual ICollection<OrderTracking> TrackingHistory { get; set; } = new HashSet<OrderTracking>();
    }
}
