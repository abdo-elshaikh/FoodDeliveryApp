﻿using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        protected new readonly ApplicationDbContext _context;
        protected new readonly ILogger<Repository<Order>> _logger;

        public OrderRepository(
            ApplicationDbContext context,
            ILogger<OrderRepository> logger) : base(context, logger)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public IQueryable<Order> GetUserOrders(string userId, params Expression<Func<Order, object>>[] includes)
        {
            try
            {
                IQueryable<Order> query = _context.Orders.Where(o => o.UserId == userId);
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for user {UserId}", userId);
                throw;
            }
        }

        public IQueryable<Order> GetAllOrders(params Expression<Func<Order, object>>[] includes)
        {
            try
            {
                IQueryable<Order> query = _context.Orders;
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
                return query;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving all orders");
                throw;
            }
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders
                .Include(o => o.User)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.MenuItem)
                .ThenInclude(oi => oi.Restaurant)
                .FirstOrDefaultAsync(o => o.Id == orderId);
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving order with ID {OrderId}", orderId);
                throw;
            }
        }

        public IQueryable<Order> GetRecentOrders(int count, params Expression<Func<Order, object>>[] includes)
        {
            IQueryable<Order> query = _context.Orders.OrderByDescending(o => o.OrderDate).Take(count);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return query;
        }

        public async Task<IEnumerable<Order>> GetOrdersByStatusAsync(OrderStatus status)
        {
            try
            {
                return await _context.Orders
                    .Include(o => o.User)
                    .Where(o => o.Status == status)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders with status {OrderStatus}", status);
                throw;
            }
        }

        public async Task<IEnumerable<Order>> GetOrdersByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate > endDate)
                {
                    _logger.LogWarning("Invalid date range: start date {StartDate} is after end date {EndDate}", startDate, endDate);
                    return Enumerable.Empty<Order>();
                }

                return await _context.Orders
                    .Include(o => o.User)
                    .Include(o => o.OrderItems)
                    .Where(o => o.OrderDate >= startDate && o.OrderDate <= endDate)
                    .OrderByDescending(o => o.OrderDate)
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders between {StartDate} and {EndDate}", startDate, endDate);
                throw;
            }
        }

        public async Task<Order?> UpdateOrderStatusAsync(int orderId, OrderStatus newStatus)
        {
            try
            {
                var order = await _context.Orders
                .Include(o => o.User)
                .ThenInclude(o => o.Addresses)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for status update", orderId);
                    return null;
                }

                order.Status = newStatus;
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for order {OrderId} to {NewStatus}", orderId, newStatus);
                throw;
            }
        }

        public async Task<Order?> CancelOrderAsync(int orderId)
        {
            try
            {
                var order = await _context.Orders
                .Include(o => o.User)
                .ThenInclude(o => o.Addresses)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for cancellation", orderId);
                    return null;
                }

                order.Status = OrderStatus.Canceled;
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order {OrderId}", orderId);
                throw;
            }
            ;
        }

        public async Task<IEnumerable<Order>> GetOrdersByRestaurantAsync(int restaurantId)
        {
            try
            {
                return await _context.Orders
                   .Include(o => o.User)
                   .Include(o => o.OrderItems)
                   .ThenInclude(oi => oi.MenuItem)
                   .ThenInclude(oi => oi.Restaurant)
                   .Where(o => o.OrderItems.Any(oi => oi.MenuItem.RestaurantId == restaurantId))
                   .OrderByDescending(o => o.OrderDate)
                   .ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error retrieving orders for restaurant {RestaurantId}", restaurantId);
                throw;
            }
        }

        public async Task<Order> SetDeliveryTimeAsync(int orderId, DateTime deliveryTime)
        {
            try
            {
                var order = await _context.Orders
                .Include(o => o.User)
                .ThenInclude(o => o.Addresses)
                .Include(o => o.OrderItems)
                .FirstOrDefaultAsync(o => o.Id == orderId);

                if (order == null)
                {
                    _logger.LogWarning("Order with ID {OrderId} not found for delivery time update", orderId);
                    return null;
                }
                order.EstimatedDeliveryTime = deliveryTime;
                await _context.SaveChangesAsync();
                return order;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating delivery time for order {OrderId} to {DeliveryTime}", orderId, deliveryTime);
                throw;
            }
        }
    }
}
