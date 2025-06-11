using FoodDeliveryApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        Task<IEnumerable<Review>> GetByRestaurantAsync(int restaurantId);
        Task<IEnumerable<Review>> GetByMenuItemAsync(int MenuItemId);
        Task<double?> GetAverageRatingAsync(int? restaurantId, int? MenuItemId);
        Task<IEnumerable<Review>> GetRecentReviewsAsync(int count);
        Task<IEnumerable<Review>> GetByUserIdAsync(string userId);
        Task<IEnumerable<Review>> GetRestaurantReviewsAsync(int restaurantId);
    }
}
