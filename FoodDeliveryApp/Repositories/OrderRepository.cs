using FoodDeliveryApp.Models;
using FoodDeliveryApp.Data;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories
{
    public class OrderRepository : Repository<Order>, IOrderRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Order GetByOrderId(int orderId)
        {
            return _context.Orders.Include(o => o.OrderDetails).ThenInclude(od => od.Item).FirstOrDefault(o => o.OrdId == orderId);
        }
    }
}

