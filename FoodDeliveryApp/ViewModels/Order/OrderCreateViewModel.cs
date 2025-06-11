using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.Menu;

namespace FoodDeliveryApp.ViewModels.Order
{
    public class OrderCreateViewModel
    {
        [Required]
        public int DeliveryAddressId { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; }

        [StringLength(500)]
        public string DeliveryInstructions { get; set; }

        [StringLength(500)]
        public string SpecialRequests { get; set; }

        public string PromoCode { get; set; }

        public List<OrderItemCreateViewModel> Items { get; set; } = new List<OrderItemCreateViewModel>();

        // Calculated properties
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        // Validation
        public bool IsValid => Items != null && Items.Count > 0;

        // Delivery address options
        public List<AddressViewModel> DeliveryAddresses { get; set; } = new List<AddressViewModel>();
    }

    public class OrderItemCreateViewModel
    {
        [Required]
        public int MenuItemId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(1, 100)]
        public int Quantity { get; set; }

        [StringLength(200)]
        public string SpecialInstructions { get; set; }

        public List<string> Modifiers { get; set; }

        // Display properties
        public string MenuItemName { get; set; }
        public string RestaurantName { get; set; }
        public string ImageUrl { get; set; }

        // Calculated properties
        public decimal UnitPrice { get; set; }
        public decimal TotalPrice => UnitPrice * Quantity;
    }
} 