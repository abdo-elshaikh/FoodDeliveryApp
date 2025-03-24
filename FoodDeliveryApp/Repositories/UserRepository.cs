using Microsoft.EntityFrameworkCore;
using FoodDeliveryApp.Data;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public class UserRepository: Repository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public User GetByUserId(string userId)
        {
            return _context.Users.FirstOrDefault(u => u.UserId == userId);
        }

    }
}