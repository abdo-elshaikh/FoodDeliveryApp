using System;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.OrderViewModels
{
    public class OrderManagementViewModel
    {
        public int Id { get; set; }

        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime OrderDate { get; set; }

        [Display(Name = "Customer")]
        public string CustomerName { get; set; } = string.Empty;

        [Display(Name = "Restaurants")]
        public int RestaurantCount { get; set; }

        [Display(Name = "Items")]
        public int ItemsCount { get; set; }

        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }

        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }

        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        public string StatusBadgeClass => GetStatusBadgeClass(Status);

        public string PaymentStatusText => GetPaymentStatusText(PaymentStatus);

        private static string GetStatusBadgeClass(OrderStatus status)
        {
            return status switch
            {
                OrderStatus.Placed => "bg-warning",
                OrderStatus.Confirmed => "bg-primary",
                OrderStatus.InPreparation => "bg-info",
                OrderStatus.ReadyForPickup => "bg-warning",
                OrderStatus.OutForDelivery => "bg-info",
                OrderStatus.Delivered => "bg-success",
                OrderStatus.Canceled => "bg-danger",
                _ => "bg-secondary"
            };
        }

        private static string GetPaymentStatusText(PaymentStatus status)
        {
            return status switch
            {
                PaymentStatus.Pending => "Payment pending",
                PaymentStatus.Paid => "Paid",
                PaymentStatus.Failed => "Payment failed",
                PaymentStatus.Refunded => "Refunded",
                _ => "Unknown"
            };
        }
    }

    public class OrderManagementListViewModel
    {
        public List<OrderManagementViewModel> Orders { get; set; } = new List<OrderManagementViewModel>();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? StatusFilter { get; set; } = "All";
        public int? RestaurantId { get; set; }
        public string? RestaurantName { get; set; }
        public string? SortBy { get; set; } = "OrderDate";
        public string? SortOrder { get; set; } = "desc";
        public string? SearchQuery { get; set; }
    }
}