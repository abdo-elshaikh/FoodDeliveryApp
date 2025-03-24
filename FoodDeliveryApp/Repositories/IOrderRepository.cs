using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public interface IOrderRepository : IRepository<Order>
    {
        Order GetByOrderId(int orderId);
    }
}
