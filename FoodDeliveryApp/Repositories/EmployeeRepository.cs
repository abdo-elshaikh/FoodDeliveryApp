using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.Data;

namespace FoodDeliveryApp.Repositories
{
    public class EmployeeRepository : Repository<Employee>, IEmployeeRepository
    {
        private readonly ApplicationDbContext _context;
        public EmployeeRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
        public Employee GetByEmpId(int empId)
        {
            return _context.Employees.Include(e => e.User).FirstOrDefault(e => e.EmployeeId == empId);
        }
        public Employee GetByUserId(string userId)
        {
            return _context.Employees.Include(e => e.User).FirstOrDefault(e => e.UserId == userId);
        }
        public IEnumerable<Employee> GetByCategory(EmployeeCategory category)
        {
            return _context.Employees.Include(e => e.User).Where(e => e.EmpCategory == category).ToList();
        }
        public IEnumerable<Employee> GetByName(string name)
        {
            return _context.Employees.Include(e => e.User).Where(e => e.FirstName.Contains(name) || e.LastName.Contains(name)).ToList();
        }
        public IEnumerable<Employee> GetByStatus(bool isActive)
        {
            return _context.Employees.Include(e => e.User).Where(e => e.IsActive == isActive).ToList();
        }
        public IEnumerable<Employee> GetByDateRange(DateTime startDate, DateTime endDate)
        {
            return _context.Employees.Include(e => e.User).Where(e => e.CreatedAt >= startDate && e.CreatedAt <= endDate).ToList();
        }
    }
}
