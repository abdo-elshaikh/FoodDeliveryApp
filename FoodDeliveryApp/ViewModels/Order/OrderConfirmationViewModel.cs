using FoodDeliveryApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Order
{
    /// <summary>
    /// View model for displaying order confirmation details
    /// </summary>
    public class OrderConfirmationViewModel : OrderViewModelBase
    {
        // Total price of the order
        [Required]
        [Display(Name = "Total Price")]
        public decimal TotalPrice { get; set; }
        [Required]
        [Display(Name = "Payment Method")]
        public new PaymentMethod PaymentMethod { get; set; }

        [Required]
        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }

        [Required]
        [StringLength(500)]
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;

        [Display(Name = "Estimated Delivery Time")]
        [DataType(DataType.DateTime)]
        public DateTime? EstimatedDeliveryTime { get; set; }

        [Display(Name = "Actual Delivery Time")]
        [DataType(DataType.DateTime)]
        public DateTime? ActualDeliveryTime { get; set; }

        [StringLength(500)]
        [Display(Name = "Delivery Instructions")]
        public string DeliveryInstructions { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Order Items")]
        public List<OrderItemViewModel> OrderItems { get; set; } = new();

        [Required]
        [StringLength(100)]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Restaurant Phone")]
        public string RestaurantPhone { get; set; } = string.Empty;

        [Required]
        [StringLength(500)]
        [Display(Name = "Restaurant Address")]
        public string RestaurantAddress { get; set; } = string.Empty;

        [Display(Name = "Driver")]
        public DriverInfo? Driver { get; set; }

        [Display(Name = "Is Delayed")]
        public bool IsDelayed { get; set; }

        [StringLength(500)]
        [Display(Name = "Delay Reason")]
        public string? DelayReason { get; set; }

        [Display(Name = "Requires Signature")]
        public bool RequiresSignature { get; set; }

        [StringLength(200)]
        [Display(Name = "Signature Image URL")]
        public string? SignatureImageUrl { get; set; }

        [Display(Name = "Delivery Photos")]
        public List<string> DeliveryPhotos { get; set; } = new();

        [Display(Name = "Custom Fields")]
        public Dictionary<string, string> CustomFields { get; set; } = new();

        [Display(Name = "Status Badge Class")]
        public string StatusBadgeClass => GetStatusBadgeClass(Status);

        private static string GetStatusBadgeClass(OrderStatus status) => status switch
        {
            OrderStatus.Pending => "badge bg-secondary",
            OrderStatus.Confirmed => "badge bg-primary",
            OrderStatus.InPreparation => "badge bg-info",
            OrderStatus.ReadyForPickup => "badge bg-warning",
            OrderStatus.OutForDelivery => "badge bg-info",
            OrderStatus.Delivered => "badge bg-success",
            OrderStatus.Canceled => "badge bg-danger",
            _ => "badge bg-secondary"
        };
    }

    public class DriverInfo
    {
        public string Name { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string VehicleInfo { get; set; } = string.Empty;
        public double? CurrentLatitude { get; set; }
        public double? CurrentLongitude { get; set; }
        public string? CurrentLocation { get; set; }
        public DateTime? LastLocationUpdate { get; set; }
    }

    public class OrderItemInfo
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string SpecialInstructions { get; set; } = string.Empty;
        public List<string> SelectedOptions { get; set; } = new();
        public bool IsPrepared { get; set; }
        public DateTime? PreparedAt { get; set; }
    }
} 