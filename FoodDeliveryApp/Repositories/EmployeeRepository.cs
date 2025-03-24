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
        public Employee GetByEmpId(string empId)
        {
            return _context.Employees.Include(e => e.User).FirstOrDefault(e => e.EmpId == empId);
        }
    }
}
