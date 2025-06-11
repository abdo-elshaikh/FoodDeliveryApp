using System.Collections.Generic;
using System.Threading.Tasks;
using FoodDeliveryApp.ViewModels;
using FoodDeliveryApp.ViewModels.Promotion;

namespace FoodDeliveryApp.Services
{
    public interface IPromotionService
    {
        Task<List<PromotionViewModel>> GetActivePromotionsAsync();
        Task<PromotionViewModel> GetPromotionByIdAsync(int id);
        Task<PromotionViewModel> GetPromotionByCodeAsync(string code);
        Task<bool> ValidatePromotionAsync(string code, decimal orderAmount);
    }
} 