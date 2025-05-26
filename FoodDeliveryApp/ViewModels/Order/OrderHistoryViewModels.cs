using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.OrderViewModels
{
    public class OrderHistoryViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; } = string.Empty;
        
        [Display(Name = "Restaurant")]
        public string RestaurantName { get; set; } = string.Empty;
        
        [Display(Name = "Order Date")]
        public DateTime OrderDate { get; set; }
        
        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }
        
        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get; set; }
        
        [Display(Name = "Items Count")]
        public int ItemsCount { get; set; }
        
        [Display(Name = "Delivery Date")]
        public DateTime? DeliveryDate { get; set; }
        
        public bool CanReview { get; set; }
        public bool IsReviewed { get; set; }
        
        public string StatusBadgeClass => GetStatusBadgeClass(Status);
        
        private static string GetStatusBadgeClass(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Placed => "badge bg-secondary",
                OrderStatus.Confirmed => "badge bg-primary",
                OrderStatus.InPreparation => "badge bg-info",
                OrderStatus.ReadyForPickup => "badge bg-warning",
                OrderStatus.OutForDelivery => "badge bg-info",
                OrderStatus.Delivered => "badge bg-success",
                OrderStatus.Canceled => "badge bg-danger",
                _ => "badge bg-secondary"
            };
        }
    }
    
    public class DeliveryHistoryViewModel
    {
        public int OrderId { get; set; }
        
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; } = string.Empty;
        
        [Display(Name = "Customer")]
        public string CustomerName { get; set; } = string.Empty;
        
        [Display(Name = "Restaurant")]
        public string RestaurantName { get; set; } = string.Empty;
        
        [Display(Name = "Pickup Time")]
        public DateTime? PickupTime { get; set; }
        
        [Display(Name = "Delivery Time")]
        public DateTime? DeliveryTime { get; set; }
        
        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }
        
        [Display(Name = "Earnings")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Earnings { get; set; }
        
        public bool IsRated { get; set; }
        public double? Rating { get; set; }
        
        public string StatusBadgeClass => GetStatusBadgeClass(Status);
        
        private static string GetStatusBadgeClass(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Placed => "badge bg-secondary",
                OrderStatus.Confirmed => "badge bg-primary",
                OrderStatus.InPreparation => "badge bg-info",
                OrderStatus.ReadyForPickup => "badge bg-warning",
                OrderStatus.OutForDelivery => "badge bg-info",
                OrderStatus.Delivered => "badge bg-success",
                OrderStatus.Canceled => "badge bg-danger",
                _ => "badge bg-secondary"
            };
        }
    }
} 