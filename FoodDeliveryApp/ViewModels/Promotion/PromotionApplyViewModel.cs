using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Promotion
{
    public class PromotionApplyViewModel
    {
        [Required(ErrorMessage = "Promo code is required")]
        [Display(Name = "Promo Code")]
        public string PromoCode { get; set; } = string.Empty;
        
        public int RestaurantId { get; set; }
        
        [Display(Name = "Total Amount")]
        public decimal TotalAmount { get; set; }
        
        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }
        
        [Display(Name = "Final Amount")]
        public decimal FinalAmount { get; set; }
        
        public bool IsPromoApplied { get; set; }
    }
} 