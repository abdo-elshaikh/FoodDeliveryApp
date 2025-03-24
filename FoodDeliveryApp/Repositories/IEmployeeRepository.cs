using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee GetByEmpId(string empId);
    }
}
