using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.PromotionViewModels
{
    public class PromotionViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        [Required]
        [StringLength(255)]
        public string Description { get; set; }

        [Required]
        [Range(0.01, 100)]
        public decimal DiscountValue { get; set; }

        [Display(Name = "Discount Type")]
        public bool IsPercentage { get; set; }

        [Display(Name = "Valid From")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Valid To")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Minimum Order")]
        [DataType(DataType.Currency)]
        public decimal? MinimumOrderAmount { get; set; }

        [Display(Name = "Usage Limit")]
        public int? UsageLimit { get; set; }

        [Display(Name = "Restaurant")]
        public int? RestaurantId { get; set; }
        public string RestaurantName { get; set; }

        public bool IsActive { get; set; }
    }

    public class PromotionCreateViewModel
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }
        [Required]
        [StringLength(255)]
        public string Description { get; set; }
        [Required]
        [Range(0.01, 100)]
        public decimal DiscountValue { get; set; }
        [Display(Name = "Discount Type")]
        public bool IsPercentage { get; set; }
        [Display(Name = "Valid From")]
        public DateTime StartDate { get; set; }
        [Display(Name = "Valid To")]
        public DateTime EndDate { get; set; }
        [Display(Name = "Minimum Order")]
        [DataType(DataType.Currency)]
        public int? MinimumOrderAmount { get; set; }
        [Display(Name = "Usage Limit")]
        public int? UsageLimit { get; set; }
        [Display(Name = "Restaurant")]
        public int? RestaurantId { get; set; }
    }

    public class PromotionEditViewModel : PromotionCreateViewModel
    {
        public int Id { get; set; }
        public bool IsActive { get; set; }
    }

    // list of promotions for a restaurant
    public class RestaurantPromotionsViewModel
    {
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public List<PromotionViewModel> Promotions { get; set; } = new List<PromotionViewModel>();
    }

    public class PromotionUsageViewModel
    {
        public int PromotionId { get; set; }
        public int RestaurantId { get; set; }
        public string UserId { get; set; }
        public string Code { get; set; }

    }

    public class ApplyPromoCodeViewModel
    {
        [Required]
        [StringLength(50)]
        public string Code { get; set; }

        public int? RestaurantId { get; set; }
        public decimal OrderSubtotal { get; set; }
    }

    public class PromotionApplicationResult
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public decimal DiscountAmount { get; set; }
        // Other relevant properties
        public decimal FinalAmount { get; set; }
        public decimal OrderSubtotal { get; set; }
        public string PromoCode { get; set; }
        public int? RestaurantId { get; set; }
        public int? PromotionId { get; set; }
        public bool IsPercentage { get; set; }
    }
}
