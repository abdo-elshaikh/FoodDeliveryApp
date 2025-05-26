using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Promotion
{
    public class PromotionEditViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Promotion code is required")]
        [StringLength(50, ErrorMessage = "Code cannot exceed 50 characters")]
        [Display(Name = "Promotion Code")]
        public string Code { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Discount value is required")]
        [Range(0.01, 10000, ErrorMessage = "Discount value must be between 0.01 and 10000")]
        [Display(Name = "Discount Value")]
        public decimal DiscountValue { get; set; }
        
        [Display(Name = "Is Percentage")]
        public bool IsPercentage { get; set; }
        
        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.DateTime)]
        public DateTime StartDate { get; set; }
        
        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.DateTime)]
        public DateTime EndDate { get; set; }
        
        [Display(Name = "Minimum Order Amount")]
        [Range(0, 10000, ErrorMessage = "Minimum order amount must be between 0 and 10000")]
        public decimal MinimumOrderAmount { get; set; }
        
        [Display(Name = "Usage Limit")]
        [Range(0, 10000, ErrorMessage = "Usage limit must be between 0 and 10000")]
        public int? UsageLimit { get; set; }
        
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        
        public int RestaurantId { get; set; }
    }
} 