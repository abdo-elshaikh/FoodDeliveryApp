using FoodDeliveryApp.ViewModels.Promotion;
using System.Collections.Generic;

namespace FoodDeliveryApp.ViewModels.PromotionViewModels
{
    public class RestaurantPromotionsViewModel
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public List<PromotionViewModel> Promotions { get; set; } = new List<PromotionViewModel>();
    }
} 