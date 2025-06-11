using Microsoft.AspNetCore.SignalR;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services.Interfaces;
using System;

namespace FoodDeliveryApp.Hubs
{
    public class OrderTrackingHub : Hub
    {
        private readonly IOrderService _orderService;
        private readonly IDriverService _driverService;

        public OrderTrackingHub(IOrderService orderService, IDriverService driverService)
        {
            _orderService = orderService;
            _driverService = driverService;
        }

        public override async Task OnConnectedAsync()
        {
            var orderId = Context.GetHttpContext()?.Request.Query["orderId"].ToString();
            if (!string.IsNullOrEmpty(orderId))
            {
                await Groups.AddToGroupAsync(Context.ConnectionId, $"Order_{orderId}");
            }
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var orderId = Context.GetHttpContext()?.Request.Query["orderId"].ToString();
            if (!string.IsNullOrEmpty(orderId))
            {
                await Groups.RemoveFromGroupAsync(Context.ConnectionId, $"Order_{orderId}");
            }
            await base.OnDisconnectedAsync(exception);
        }

        public async Task UpdateOrderStatus(int orderId, OrderStatus status, string notes = null)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order != null)
            {
                await _orderService.UpdateOrderStatusAsync(orderId, status);
                await Clients.Group($"Order_{orderId}").SendAsync("UpdateOrderStatus", new
                {
                    id = orderId,
                    status = status,
                    timestamp = DateTime.UtcNow,
                    notes = notes
                });
            }
        }

        public async Task UpdateDriverLocation(int orderId, double latitude, double longitude)
        {
            var order = await _orderService.GetOrderByIdAsync(orderId);
            if (order != null && order.DriverId != null)
            {
                await _driverService.UpdateDriverLocationAsync(order.DriverId ?? 0, latitude, longitude);
                await Clients.Group($"Order_{orderId}").SendAsync("UpdateDriverLocation", new
                {
                    id = orderId,
                    latitude = latitude,
                    longitude = longitude
                });
            }
        }
    }
}