using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
        Task<Promotion> GetByCodeAsync(string code);
        Task<IEnumerable<Promotion>> GetByRestaurantIdAsync(int restaurantId);
        Task<bool> IsCodeUniqueAsync(string code);
        Task<IEnumerable<Promotion>> GetExpiredPromotionsAsync();
        Task<(bool isValid, string errorMessage)> IsPromotionValidAsync(string code, decimal orderAmount, int? restaurantId = null);

    }
}
