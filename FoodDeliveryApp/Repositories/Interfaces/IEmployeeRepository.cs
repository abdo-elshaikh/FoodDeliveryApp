using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IEmployeeRepository : IRepository<EmployeeProfile>
    {
        Task<EmployeeProfile> GetByIdAsync(int id);
        Task<EmployeeProfile> GetByEmailAsync(string email);
        IAsyncEnumerable<EmployeeProfile> GetByPositionAsync(EmployeePosition position);
        IAsyncEnumerable<EmployeeProfile> GetByNameAsync(string name);
        IAsyncEnumerable<EmployeeProfile> GetByStatusAsync(bool isActive);
        Task<EmployeeProfile> GetByUserIdAsync(string userId);
        IAsyncEnumerable<EmployeeProfile> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    }
}
