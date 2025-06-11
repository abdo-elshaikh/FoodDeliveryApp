using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;
using FoodDeliveryApp.ViewModels.Restaurant;

namespace FoodDeliveryApp.ViewModels.Order
{
    public class OrderViewModelBase
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime CreatedAt { get; set; }
        public OrderStatus Status { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public bool CanBeCancelled { get; set; }
    }

    public class OrderDetailsViewModel : OrderViewModelBase
    {
        public DateTime? EstimatedDeliveryTime { get; set; }
        public DateTime? ActualDeliveryTime { get; set; }
        public bool IsDelayed { get; set; }
        public string DelayReason { get; set; }
        public AddressViewModel DeliveryAddress { get; set; }
        public PaymentStatus PaymentStatus { get; set; }
        public List<RestaurantOrderGroup> RestaurantGroups { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public DriverViewModel Driver { get; set; }
        // Order Items
        public List<OrderItemViewModel> Items { get; set; }
        public List<OrderTrackingViewModel> TrackingHistory { get; set; }
    }

    public class RestaurantOrderGroup
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string RestaurantPhone { get; set; }
        public string ImageUrl { get; set; }
        public AddressViewModel RestaurantAddress { get; set; }
        public List<OrderItemViewModel> Items { get; set; }
        public decimal Subtotal { get; set; }
        public OrderItemStatus Status { get; set; }
    }

    public class OrderItemViewModel
    {
        public int MenuItemId { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public decimal Subtotal { get; set; }
        public string SpecialInstructions { get; set; }
        public OrderItemStatus Status { get; set; }
        // Restaurant
        public RestaurantViewModel Restaurant { get; set; } = null!;
    }

    public class OrderListViewModel
    {
        public List<OrderSummaryViewModel> Orders { get; set; }
        public string SearchTerm { get; set; }
        public OrderStatus? Status { get; set; }
        public DateTime? Date { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int CurrentPage { get; set; }
        public int TotalPages { get; set; }
        public bool IsAdmin { get; set; }
        public bool IsRestaurantOwner { get; set; }
    }

    public class OrderSummaryViewModel : OrderViewModelBase
    {
        public List<RestaurantOrderGroup> RestaurantGroups { get; set; }
    }

    public class OrderTrackingsViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public OrderStatus Status { get; set; }
        public string DeliveryAddress { get; set; }
        public string RestaurantName { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public List<OrderItemViewModel> OrderItems { get; set; }
        public decimal Subtotal { get; set; }
        public decimal DeliveryFee { get; set; }
        public decimal Discount { get; set; }
        public decimal Total { get; set; }
        public double RestaurantLatitude { get; set; }
        public double RestaurantLongitude { get; set; }
        public double DeliveryLatitude { get; set; }
        public double DeliveryLongitude { get; set; }
        public double? DriverLatitude { get; set; }
        public double? DriverLongitude { get; set; }
        public OrderDriverViewModel Driver { get; set; }
        public bool CanCancel => Status == OrderStatus.Pending || Status == OrderStatus.Confirmed;
    }

    public class OrderDriverViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public string VehicleType { get; set; }
        public string ProfileImageUrl { get; set; }
    }
} 