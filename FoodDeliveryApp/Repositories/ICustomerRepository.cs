using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public interface ICustomerRepository : IRepository<Customer>
    {
        Customer GetByCustId(int custId);
    }
}

