using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Promotion
{
    public class PromotionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Discount type is required")]
        [Display(Name = "Discount Type")]
        public DiscountType DiscountType { get; set; }

        [Required(ErrorMessage = "Discount value is required")]
        [Range(0, 100, ErrorMessage = "Discount value must be between 0 and 100")]
        [Display(Name = "Discount Value")]
        public decimal DiscountValue { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Minimum Order Amount")]
        [Range(0, 1000, ErrorMessage = "Minimum order amount must be between 0 and 1000")]
        public decimal? MinimumOrderAmount { get; set; }

        [Display(Name = "Maximum Discount Amount")]
        [Range(0, 1000, ErrorMessage = "Maximum discount amount must be between 0 and 1000")]
        public decimal? MaximumDiscountAmount { get; set; }

        [Display(Name = "Promotion Code")]
        [StringLength(20, ErrorMessage = "Promotion code cannot exceed 20 characters")]
        public string? PromotionCode { get; set; }
        
        [Display(Name = "Code")]
        [StringLength(20, ErrorMessage = "Promotion code cannot exceed 20 characters")]
        public string? Code { get; set; }
        
        // Used for compatibility with older code
        public bool IsPercentage => DiscountType == DiscountType.Percentage;
        
        [Display(Name = "Restaurant")]
        public int? RestaurantId { get; set; }
        
        [Display(Name = "Restaurant Name")]
        public string? RestaurantName { get; set; }

        [Display(Name = "Usage Limit")]
        [Range(0, int.MaxValue, ErrorMessage = "Usage limit must be greater than or equal to 0")]
        public int? UsageLimit { get; set; }

        [Display(Name = "Usage Count")]
        public int UsageCount { get; set; }

        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public List<int>? ApplicableRestaurantIds { get; set; }
        public List<int>? ApplicableMenuItemIds { get; set; }
        public List<int>? ApplicableCategoryIds { get; set; }
        public string FormattedDiscount => IsPercentage ? $"{DiscountValue}%" : $"${DiscountValue}";
        public bool IsExpired => DateTime.Now > EndDate;
        public bool IsComingSoon => DateTime.Now < StartDate;
        public bool IsCurrentlyActive => IsActive && !IsExpired && !IsComingSoon;
    }

    public class CreatePromotionViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(100, ErrorMessage = "Title cannot exceed 100 characters")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Discount type is required")]
        [Display(Name = "Discount Type")]
        public DiscountType DiscountType { get; set; }

        [Required(ErrorMessage = "Discount value is required")]
        [Range(0, 100, ErrorMessage = "Discount value must be between 0 and 100")]
        [Display(Name = "Discount Value")]
        public decimal DiscountValue { get; set; }

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        public DateTime EndDate { get; set; }

        [Display(Name = "Minimum Order Amount")]
        [Range(0, 1000, ErrorMessage = "Minimum order amount must be between 0 and 1000")]
        public decimal? MinimumOrderAmount { get; set; }

        [Display(Name = "Maximum Discount Amount")]
        [Range(0, 1000, ErrorMessage = "Maximum discount amount must be between 0 and 1000")]
        public decimal? MaximumDiscountAmount { get; set; }

        [Display(Name = "Promotion Code")]
        [StringLength(20, ErrorMessage = "Promotion code cannot exceed 20 characters")]
        public string? PromotionCode { get; set; }

        [Display(Name = "Usage Limit")]
        [Range(0, int.MaxValue, ErrorMessage = "Usage limit must be greater than or equal to 0")]
        public int? UsageLimit { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsPublic { get; set; } = true;
        public List<int>? ApplicableRestaurantIds { get; set; }
        public List<int>? ApplicableMenuItemIds { get; set; }
        public List<int>? ApplicableCategoryIds { get; set; }
    }

    public class EditPromotionViewModel : CreatePromotionViewModel
    {
        public int Id { get; set; }
        public int UsageCount { get; set; }
    }

    public class PromotionListViewModel
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public DiscountType DiscountType { get; set; }
        public decimal DiscountValue { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? PromotionCode { get; set; }
        public bool IsActive { get; set; }
        public bool IsPublic { get; set; }
        public int UsageCount { get; set; }
        public int? UsageLimit { get; set; }
    }

    public enum DiscountType
    {
        Percentage,
        FixedAmount
    }
} 