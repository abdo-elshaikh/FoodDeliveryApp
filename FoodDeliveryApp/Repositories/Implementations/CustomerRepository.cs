using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class CustomerRepository : Repository<CustomerProfile>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<CustomerProfile> GetByUserIdAsync(string userId)
        {
            return await _context.CustomerProfiles.Include(c => c.User).FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task<CustomerProfile> GetByIdAsync(int id)
        {
            return await _context.CustomerProfiles.Include(c => c.User).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<CustomerProfile> GetByEmailAsync(string email)
        {
            return await _context.CustomerProfiles.Include(c => c.User).FirstOrDefaultAsync(c => c.User.Email == email);
        }

        public async Task<CustomerProfile> GetByPhoneNumberAsync(string phoneNumber)
        {
            return await _context.CustomerProfiles.Include(c => c.User).FirstOrDefaultAsync(c => c.PhoneNumber == phoneNumber);
        }

        public async Task<bool> CustomerExistsAsync(string userId)
        {
            return await _context.CustomerProfiles.AnyAsync(c => c.UserId == userId);
        }

        public async Task<IEnumerable<CustomerProfile>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.CustomerProfiles
                .Where(c => c.CreatedAt >= startDate && c.CreatedAt <= endDate)
                .ToListAsync();
        }
        public async Task<IEnumerable<CustomerProfile>> GetByNameAsync(string name)
        {
            return await _context.CustomerProfiles
                .Where(c => c.FirstName.Contains(name) || c.LastName.Contains(name))
                .ToListAsync();
        }

        public async Task<IEnumerable<CustomerProfile>> GetByStatusAsync(bool isActive)
        {
            return await _context.CustomerProfiles
                .Where(c => c.IsActive == isActive)
                .ToListAsync();
        }
    }
}