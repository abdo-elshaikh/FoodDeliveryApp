using System;
using System.Collections.Generic;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.Order
{
    public class OrderTrackingViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string Description { get; set; }
        public DateTime Timestamp { get; set; }
        public string Location { get; set; }
        public double? Latitude { get; set; }
        public double? Longitude { get; set; }
        public string DriverId { get; set; }
        public string DriverName { get; set; }
        public string DriverPhone { get; set; }
        public string DriverPhotoUrl { get; set; }
        public bool IsDelayed { get; set; }
        public string DelayReason { get; set; }
        public string DeliveryInstructions { get; set; } = string.Empty;
        public DateTime? EstimatedDeliveryTime { get; set; }

        // Order Details
        public string DeliveryAddress { get; set; }
        public string RestaurantName { get; set; }
        public string PaymentMethod { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; } = new List<OrderItemViewModel>();

        // Order Summary
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }

        // Map Coordinates
        public double RestaurantLatitude { get; set; }
        public double RestaurantLongitude { get; set; }
        public double DeliveryLatitude { get; set; }
        public double DeliveryLongitude { get; set; }
        public double? DriverLatitude { get; set; }
        public double? DriverLongitude { get; set; }

        // Helper Properties
        public bool CanCancel => Status == OrderStatus.Pending || Status == OrderStatus.InPreparation;
    }
}
