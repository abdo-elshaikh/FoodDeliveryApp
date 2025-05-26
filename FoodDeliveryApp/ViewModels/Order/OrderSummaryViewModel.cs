using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.OrderViewModels
{
    public class OrderSummaryViewModel
    {
        public int Id { get; set; }
        
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; } = string.Empty;
        
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime OrderDate { get; set; }
        
        [Display(Name = "Total")]
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get; set; }
        
        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }
        
        [Display(Name = "Restaurant")]
        public string RestaurantName { get; set; } = string.Empty;
        
        [Display(Name = "Items")]
        public int ItemCount { get; set; }
        
        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }
        
        public string StatusBadgeClass => GetStatusBadgeClass(Status);
        public string PaymentStatusBadgeClass => GetPaymentStatusBadgeClass(PaymentStatus);
        
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
        
        private static string GetPaymentStatusBadgeClass(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "badge bg-warning",
                PaymentStatus.Paid => "badge bg-success",
                PaymentStatus.Failed => "badge bg-danger",
                PaymentStatus.Refunded => "badge bg-info",
                _ => "badge bg-secondary"
            };
        }
    }
} 