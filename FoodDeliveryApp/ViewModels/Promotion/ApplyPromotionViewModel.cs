using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Promotion
{
    public class ApplyPromotionViewModel
    {
        [Required(ErrorMessage = "Please enter a promotion code")]
        [Display(Name = "Promotion Code")]
        public string Code { get; set; }

        [Display(Name = "Order Total")]
        public decimal OrderTotal { get; set; }

        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount { get; set; }

        [Display(Name = "Final Total")]
        public decimal FinalTotal { get; set; }

        public bool IsValid { get; set; }
        public string Message { get; set; }
    }
} 