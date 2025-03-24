using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public interface IOrderDetailRepository : IRepository<OrderDetail>
    {
        OrderDetail GetByOrderDetailId(int orderDetailId);
    }
}
