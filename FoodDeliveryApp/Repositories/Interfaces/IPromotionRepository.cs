using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Repositories.Interfaces
{
    public interface IPromotionRepository : IRepository<Promotion>
    {
        Task<Promotion> GetByCodeAsync(string code);
        Task<IEnumerable<Promotion>> GetActivePromotionsAsync();
        Task<IEnumerable<Promotion>> GetExpiredPromotionsAsync();
        Task<bool> IsPromotionValidAsync(string code, decimal orderAmount);
        // GetValidPromotion
        Task<Promotion> GetValidPromotionAsync(string code, int restaurantId, decimal subtotal);
    }
}