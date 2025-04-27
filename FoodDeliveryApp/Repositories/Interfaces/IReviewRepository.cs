using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IReviewRepository : IRepository<Review>
    {
        // add repository methods specific to Review entity
        Task AddReviewAsync(Review review);
        Task<IEnumerable<Review>> GetByRestaurantAsync(int restaurantId);
        Task<IEnumerable<Review>> GetByCustomerAsync(int customerId);
        Task<double?> GetAverageRatingAsync(int restaurantId);
        Task<IEnumerable<Review>> GetRecentReviewsAsync(int count);
    }
}