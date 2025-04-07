using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public interface IEmployeeRepository : IRepository<Employee>
    {
        Employee GetByEmpId(int empId);
        Employee GetByUserId(string userId);

        IEnumerable<Employee> GetByCategory(EmployeeCategory category);
        IEnumerable<Employee> GetByName(string name);
        IEnumerable<Employee> GetByStatus(bool isActive);
        IEnumerable<Employee> GetByDateRange(DateTime startDate, DateTime endDate);

    }
}
