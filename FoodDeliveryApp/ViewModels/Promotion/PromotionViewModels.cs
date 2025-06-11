using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Promotion
{
    /// <summary>
    /// View model for displaying promotion information
    /// </summary>
    public class PromotionViewModel : BaseViewModel
    {
        // Title
        public string Title { get; set; } = string.Empty;
        [Required(ErrorMessage = "Code is required")]
        [StringLength(100, ErrorMessage = "Code cannot exceed 100 characters")]
        [Display(Name = "Code")]
        public string Code { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Restaurant")]
        public int? RestaurantId { get; set; }

        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Discount value is required")]
        [Display(Name = "Discount Value")]
        public decimal DiscountValue { get; set; }

        [Required(ErrorMessage = "Is percentage is required")]
        [Display(Name = "Is Percentage")]
        public bool IsPercentage { get; set; }

        [Required(ErrorMessage = "Minimum order amount is required")]
        [Display(Name = "Minimum Order Amount")]
        public decimal MinimumOrderAmount { get; set; }

        [Required(ErrorMessage = "Usage limit is required")]
        [Display(Name = "Usage Limit")]
        public int? UsageLimit { get; set; }

        [Display(Name = "Usage Count")]
        public int UsageCount { get; set; }

        // DiscountAmount
        [Display(Name = "Discount Amount")]
        public decimal DiscountAmount => IsPercentage ? (DiscountValue / 100) * MinimumOrderAmount : DiscountValue;
        // ImageUrl
        [Display(Name = "Image URL")]
        public string ImageUrl { get; set; } = string.Empty;
    }

    /// <summary>
    /// View model for displaying a list of promotions with pagination and filtering
    /// </summary>
    public class PromotionListViewModel : BaseListViewModel<PromotionViewModel>
    {
        public List<PromotionViewModel> Promotions 
        { 
            get => Items; 
            set => Items = value; 
        }

        [Display(Name = "Restaurant")]
        public int? RestaurantId { get; set; }

        [Display(Name = "Active Only")]
        public bool ActiveOnly { get; set; } = true;

        [Display(Name = "Current Only")]
        public bool CurrentOnly { get; set; } = true;
    }

    /// <summary>
    /// View model for creating a new promotion
    /// </summary>
    public class PromotionCreateViewModel : BaseCreateViewModel
    {
        [Required(ErrorMessage = "Code is required")]
        [StringLength(100, ErrorMessage = "Code cannot exceed 100 characters")]
        [Display(Name = "Code")]
        public string Code { get; set; }

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; }


        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Restaurant")]
        public int? RestaurantId { get; set; }

        [Required(ErrorMessage = "Discount value is required")]
        [Display(Name = "Discount Value")]
        public decimal DiscountValue { get; set; }

        [Required(ErrorMessage = "Is percentage is required")]
        [Display(Name = "Is Percentage")]
        public bool IsPercentage { get; set; }

        [Required(ErrorMessage = "Minimum order amount is required")]
        [Display(Name = "Minimum Order Amount")]
        public decimal MinimumOrderAmount { get; set; }
    }

    /// <summary>
    /// View model for editing an existing promotion
    /// </summary>
    public class PromotionEditViewModel : BaseEditViewModel
    {
        [Required(ErrorMessage = "Code is required")]
        [StringLength(100, ErrorMessage = "Code cannot exceed 100 characters")]
        [Display(Name = "Code")]
        public string Code { get; set; } = string.Empty;

        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "Start date is required")]
        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required(ErrorMessage = "End date is required")]
        [Display(Name = "End Date")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Display(Name = "Restaurant")]
        public int? RestaurantId { get; set; }

        [Required(ErrorMessage = "Discount value is required")]
        [Display(Name = "Discount Value")]
        public decimal DiscountValue { get; set; }

        [Required(ErrorMessage = "Is percentage is required")]
        [Display(Name = "Is Percentage")]
        public bool IsPercentage { get; set; }

        [Required(ErrorMessage = "Minimum order amount is required")]
        [Display(Name = "Minimum Order Amount")]
        public decimal MinimumOrderAmount { get; set; }
    }    
} 