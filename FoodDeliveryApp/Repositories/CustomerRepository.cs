using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public class CustomerRepository : Repository<Customer>, ICustomerRepository
    {
        private readonly ApplicationDbContext _context;

        public CustomerRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Customer GetByCustId(string custId)
        {
            return _context.Customers.Include(c => c.User).FirstOrDefault(c => c.CustId == custId);
        }
    }
}