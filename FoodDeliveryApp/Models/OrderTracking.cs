using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class OrderTracking : BaseEntity
    {
        [Required]
        [ForeignKey("Order")]
        public int OrderId { get; set; }

        [Required]
        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        [Required]
        [ForeignKey("DeliveryAddress")]
        public int DeliveryAddressId { get; set; }

        [Required]
        public OrderStatus Status { get; set; }

        [Required]
        public DateTime Timestamp { get; set; } = DateTime.UtcNow;

        [StringLength(500)]
        public string? Notes { get; set; }

        public new string? UpdatedBy { get; set; }

        [StringLength(200)]
        public string? Location { get; set; }

        [NotMapped]
        [Column(TypeName = "jsonb")]
        public Dictionary<string, string> Metadata { get; set; } = new Dictionary<string, string>();

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public DateTime? EstimatedArrivalTime { get; set; }

        public bool IsDelayed { get; set; }

        [StringLength(200)]
        public string? DelayReason { get; set; }

        public bool RequiresSignature { get; set; }

        [StringLength(255)]
        public string? SignatureImageUrl { get; set; }

        [StringLength(255)]
        public string? TrackingUrl { get; set; }

        // Navigation properties
        public virtual Order Order { get; set; } = null!;
        // Driver navigation property
        public virtual Driver Driver { get; set; } = null!;
        public virtual Address DeliveryAddress { get; set; } = null!;
    }
}
