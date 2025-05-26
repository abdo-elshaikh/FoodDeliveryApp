using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Order>> GetUserOrdersAsync(string userId, params Expression<Func<Order, object>>[] includes)
        {
            IQueryable<Order> query = _context.Orders.Where(o => o.UserId == userId);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync(params Expression<Func<Order, object>>[] includes)
        {
            IQueryable<Order> query = _context.Orders;
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }

        public async Task<Order?> GetOrderWithDetailsAsync(int orderId)
        {
            return await _context.Orders
                .Include(o => o.User)
                .Include(o => o.Restaurant)
                .Include(o => o.Address)
                .Include(o => o.Payment)
                    .ThenInclude(p => p.PaymentMethod)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.MenuItem)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Customizations)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<IEnumerable<Order>> GetRecentOrdersAsync(int count, params Expression<Func<Order, object>>[] includes)
        {
            IQueryable<Order> query = _context.Orders.OrderByDescending(o => o.OrderDate).Take(count);
            foreach (var include in includes)
            {
                query = query.Include(include);
            }
            return await query.ToListAsync();
        }
    }
}
