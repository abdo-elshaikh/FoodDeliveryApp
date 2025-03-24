using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories
{
    public class OrderDetailRepository : Repository<OrderDetail>, IOrderDetailRepository
    {
        private readonly ApplicationDbContext _context;
        public OrderDetailRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public OrderDetail GetByOrderDetailId(int orderDetailId)
        {
            return _context.OrderDetails.Include(od => od.Item).FirstOrDefault(od => od.OrdDetId == orderDetailId);
        }
    }
}
