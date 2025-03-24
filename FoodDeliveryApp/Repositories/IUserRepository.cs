using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories
{
    public interface IUserRepository : IRepository<User>
    {
        User GetByUserId(string userId);
    }
}