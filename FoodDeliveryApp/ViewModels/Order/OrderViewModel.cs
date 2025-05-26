using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.Payment;
using FoodDeliveryApp.ViewModels.Restaurant;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDeliveryApp.ViewModels.OrderViewModels
{
    public class OrderDetailsViewModel
    {
        public int Id { get; set; }
        [DisplayFormat(DataFormatString = "{0:MMM dd, yyyy HH:mm}")]
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public AddressViewModel? Address { get; set; }
        public string? SpecialInstructions { get; set; }
        [Display(Name = "Payment Method")]
        public string PaymentDetails { get; set; } = string.Empty;
        public PaymentStatus PaymentStatus { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Subtotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal DeliveryFee { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Tax { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get; set; }
    }

    

    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public int MenuItemId { get; set; }
        public string Name { get; set; } = string.Empty;
        public string? ImageUrl { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string? SpecialInstructions { get; set; }
        public List<OrderCustomizationViewModel> Customizations { get; set; } = new List<OrderCustomizationViewModel>();
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Subtotal => Price * Quantity + Customizations.Sum(c => c.Price * Quantity);
        public virtual RestaurantViewModel Restaurant { get; set; } = new RestaurantViewModel();
    }

    public class OrderCustomizationViewModel
    {
        public int Id { get; set; }
        public int OptionId { get; set; }
        public int ChoiceId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g., "Toppings"
        public string Choice { get; set; } = string.Empty; // e.g., "Extra Cheese"
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Price { get; set; }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Order Number")]
        public string OrderNumber { get; set; } = string.Empty;
        [Display(Name = "Customer")]
        public string CustomerName { get; set; } = string.Empty;
        [Display(Name = "Customer Phone")]
        public string CustomerPhone { get; set; } = string.Empty;
        [Display(Name = "Restaurant")]
        public string RestaurantName { get; set; } = string.Empty;
        [Display(Name = "Restaurant Phone")]
        public string RestaurantPhone { get; set; } = string.Empty;
        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }
        [Display(Name = "Order Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}")]
        public DateTime OrderDate { get; set; }
        [Display(Name = "Delivery Date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", NullDisplayText = "Not delivered")]
        public DateTime? DeliveryDate { get; set; }
        [Display(Name = "Estimated Delivery Time")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", NullDisplayText = "Not available")]
        public DateTime? EstimatedDeliveryTime { get; set; }
        [Display(Name = "Delivery Address")]
        public string DeliveryAddress { get; set; } = string.Empty;
        [Display(Name = "Payment Method")]
        public string PaymentMethod { get; set; } = string.Empty;
        [Display(Name = "Payment Status")]
        public PaymentStatus PaymentStatus { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Subtotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Tax { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal DeliveryFee { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Discount { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get; set; }
        [Display(Name = "Delivery Instructions")]
        public string? DeliveryInstructions { get; set; }
        [Display(Name = "Special Requests")]
        public string? SpecialRequests { get; set; }
        [Display(Name = "Tracking URL")]
        public string? TrackingUrl { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
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

    public class OrderListViewModel
    {
        public List<OrderViewModel> Orders { get; set; } = new List<OrderViewModel>();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }

    public class OrderCreateViewModel
    {
        [Required(ErrorMessage = "Delivery address is required")]
        [Display(Name = "Delivery Address")]
        public int AddressId { get; set; }
        [Required(ErrorMessage = "Payment method is required")]
        [Display(Name = "Payment Method")]
        public int PaymentMethodId { get; set; }
        [Display(Name = "Delivery Instructions")]
        [StringLength(500, ErrorMessage = "Delivery instructions cannot exceed 500 characters")]
        public string? DeliveryInstructions { get; set; }
        [Display(Name = "Special Requests")]
        [StringLength(500, ErrorMessage = "Special requests cannot exceed 500 characters")]
        public string? SpecialRequests { get; set; }
        [Display(Name = "Promo Code")]
        public string? PromoCode { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<OrderItemViewModel> CartItems { get; set; } = new List<OrderItemViewModel>();
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Subtotal { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Tax { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal DeliveryFee { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Discount { get; set; }
        [DisplayFormat(DataFormatString = "{0:C}")]
        public decimal Total { get; set; }

        // OrderNotes
        [Display(Name = "Order Notes")]
        [StringLength(500, ErrorMessage = "Order notes cannot exceed 500 characters")]
        public string OrderNotes { get; set; } = string.Empty;

        // select addres id and payment method
        public int SelectedAddressId { get; set; }
        public int SelectedPaymentMethod { get; set; }

        // is new address
        public bool IsNewAddress { get; set; } = false;
        public AddressViewModel NewAddress { get; set; } = new AddressViewModel();

        // select lists for address and payment method
        [Display(Name = "Select Address")]
        [Required(ErrorMessage = "Please select an address")]
        public IEnumerable<SelectListItem>? AddressOptions { get; set; }
        public IEnumerable<SelectListItem>? PaymentMethodOptions { get; set; }
    }

    public class OrderTrackViewModel
    {
        public int OrderId { get; set; }
        public OrderStatus Status { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy hh:mm tt}", NullDisplayText = "Not available")]
        public DateTime? EstimatedDeliveryTime { get; set; }
        public AddressViewModel DeliveryAddress { get; set; } = new AddressViewModel();
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();
    }

    // CheckoutViewModel
    public class CheckoutViewModel
    {
       
        [Required(ErrorMessage = "Please select or enter a delivery address")]
        public int? SelectedAddressId { get; set; }

        [Required(ErrorMessage = "Please select a payment method")]
        public string SelectedPaymentMethod { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "Promo code is too long")]
        public string PromoCode { get; set; } = string.Empty;

        public bool IsNewAddress { get; set; }

        public AddressViewModel NewAddress { get; set; } = new AddressViewModel();

        [Display(Name = "Order Notes")]
        [StringLength(500, ErrorMessage = "Order notes cannot exceed 500 characters")]
        public string OrderNotes { get; set; } = string.Empty;

        [Display(Name = "Subtotal")]
        public decimal Subtotal => CartItems.Sum(i => i.Subtotal);

        [Display(Name = "Delivery Fee")]
        public decimal DeliveryFee { get; set; }

        [Display(Name = "Tax")]
        public decimal Tax => Subtotal * 0.08m; // Example 8% tax rate

        [Display(Name = "Discount")]
        public decimal Discount { get; set; }

        [Display(Name = "Total")]
        public decimal Total => Subtotal + DeliveryFee + Tax - Discount;
        public List<OrderItemViewModel> CartItems { get; set; } = new List<OrderItemViewModel>();

        // address options
        public IEnumerable<SelectListItem>? AddressOptions { get; set; }
        // payment method options
        public IEnumerable<SelectListItem>? PaymentMethodOptions { get; set; }
    }

}