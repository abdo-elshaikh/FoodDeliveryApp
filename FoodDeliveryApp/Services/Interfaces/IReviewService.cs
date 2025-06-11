using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Home;

namespace FoodDeliveryApp.Services
{
    public interface IReviewService
    {
        Task<List<ReviewViewModel>> GetRecentReviewsAsync(int count);
        Task<List<ReviewViewModel>> GetRestaurantReviewsAsync(int restaurantId, int page = 1, int pageSize = 10);
        Task<ReviewViewModel> GetReviewByIdAsync(int id);
        Task<ReviewViewModel> CreateReviewAsync(ReviewViewModel review);
        Task<bool> UpdateReviewAsync(ReviewViewModel review);
        Task<bool> DeleteReviewAsync(int id);
    }
} 