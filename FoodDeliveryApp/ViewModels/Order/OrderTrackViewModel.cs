using System;
using System.Collections.Generic;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;

namespace FoodDeliveryApp.ViewModels.Order
{
    public class OrderTrackViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public decimal TotalPrice { get; set; }
        // EstimatedDeliveryTime
        public DateTime? EstimatedDeliveryTime { get; set; }

        public TimeSpan? RemainingTime { get; set; }
        public AddressViewModel DeliveryAddress { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
        public OrderTrackingViewModel LatestTracking { get; set; }
        public List<OrderTrackingViewModel> TrackingHistory { get; set; }
        public bool ShowMap { get; set; }
        public bool IsDelivered { get; set; }
        public bool IsCancelled { get; set; }
    }
} 