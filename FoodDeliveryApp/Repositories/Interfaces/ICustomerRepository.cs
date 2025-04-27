using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface ICustomerRepository : IRepository<CustomerProfile>
    {
        Task<CustomerProfile> GetByUserIdAsync(string userId);
        Task<CustomerProfile> GetByIdAsync(int id);
        Task<CustomerProfile> GetByEmailAsync(string email);
        Task<CustomerProfile> GetByPhoneNumberAsync(string phoneNumber);
        Task<bool> CustomerExistsAsync(string userId);
        Task<IEnumerable<CustomerProfile>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<IEnumerable<CustomerProfile>> GetByNameAsync(string name);
        Task<IEnumerable<CustomerProfile>> GetByStatusAsync(bool isActive);
    }
}

