using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace FoodDeliveryApp.Repositories.Implementations
{
    public class EmployeeRepository : Repository<EmployeeProfile>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async IAsyncEnumerable<EmployeeProfile> GetByPositionAsync(EmployeePosition postion)
        {
            var employees = _context.EmployeeProfiles
                .Where(e => e.Position == postion)
                .AsAsyncEnumerable();
            await foreach (var employee in employees)
            {
                yield return employee;
            }
        }

        public async IAsyncEnumerable<EmployeeProfile> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            var employees = _context.EmployeeProfiles
                .Where(e => e.CreatedAt >= startDate && e.CreatedAt <= endDate)
                .AsAsyncEnumerable();
            await foreach (var employee in employees)
            {
                yield return employee;
            }
        }

        public async Task<EmployeeProfile> GetByEmailAsync(string email)
        {
            return await _context.EmployeeProfiles
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.User.Email == email);
        }


        public async Task<EmployeeProfile> GetByIdAsync(int id)
        {
            return await _context.EmployeeProfiles
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        public async IAsyncEnumerable<EmployeeProfile> GetByNameAsync(string name)
        {
            var employees = _context.EmployeeProfiles
                .Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name))
                .AsAsyncEnumerable();
            await foreach (var employee in employees)
            {
                yield return employee;
            }
        }

        public async IAsyncEnumerable<EmployeeProfile> GetByStatusAsync(bool isActive)
        {
            var employees = _context.EmployeeProfiles
                .Where(e => e.IsActive == isActive)
                .AsAsyncEnumerable();
            await foreach (var employee in employees)
            {
                yield return employee;
            }
        }

        public async Task<EmployeeProfile> GetByUserIdAsync(string userId)
        {
            return await _context.EmployeeProfiles
                .Include(e => e.User)
                .FirstOrDefaultAsync(e => e.UserId == userId);
        }
    }
}
