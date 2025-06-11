using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Promotion
{
    public class PromotionApplyViewModel
    {
        [Required(ErrorMessage = "Promotion code is required")]
        [StringLength(50, ErrorMessage = "Promotion code cannot exceed 50 characters")]
        public string Code { get; set; }

        [Required]
        public decimal OrderTotal { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal FinalTotal { get; set; }

        public bool IsValid { get; set; }

        public string Message { get; set; }
    }
} 