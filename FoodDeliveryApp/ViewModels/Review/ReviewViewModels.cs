using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Review
{
    public class RestaurantReviewViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Restaurant is required")]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        [Required(ErrorMessage = "Restaurant name is required")]
        [Display(Name = "Restaurant Name")]
        public string RestaurantName { get; set; } = "Restaurant Name";
        public string UserName { get; set; } = "Unknown User";
        public string UserImageUrl { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public bool IsVerified { get; set; }
        public List<string> Images { get; set; } = new();

        internal object ToList()
        {
            throw new NotImplementedException();
        }
    }

    public class MenuItemReviewViewModel : BaseViewModel
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "User is required")]
        [Display(Name = "User")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Menu item is required")]
        [Display(Name = "Menu Item")]
        public int MenuItemId { get; set; }

        [Required(ErrorMessage = "Menu item name is required")]
        [Display(Name = "Menu Item Name")]
        public string MenuItemName { get; set; }
        public string UserName { get; set; }
    }

    public class ReviewListViewModel
    {
        public RestaurantReviewListViewModel restaurantReviewListViewModel { get; set; } = new RestaurantReviewListViewModel();
        public MenuItemReviewListViewModel menuItemReviewListViewModel { get; set; } = new MenuItemReviewListViewModel();
    }

    public class RestaurantReviewListViewModel : BaseListViewModel<RestaurantReviewViewModel>
    {
    }
    public class MenuItemReviewListViewModel : BaseListViewModel<MenuItemReviewViewModel>
    {
    }

    public class RestaurantReviewCreateViewModel
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters")]
        [Display(Name = "Content")]
        public string Content { get; set; }
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Restaurant ID is required")]
        [Display(Name = "Restaurant ID")]
        public int RestaurantId { get; set; }
    }

    public class MenuItemReviewCreateViewModel
    {
        [Required(ErrorMessage = "Content is required")]
        [StringLength(1000, ErrorMessage = "Content cannot exceed 1000 characters")]
        [Display(Name = "Content")]
        public string Content { get; set; }

        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        [Display(Name = "Rating")]
        public double Rating { get; set; }

        [Required(ErrorMessage = "User ID is required")]
        [Display(Name = "User ID")]
        public string UserId { get; set; }

        [Required(ErrorMessage = "Menu Item ID is required")]
        [Display(Name = "Menu Item ID")]
        public int MenuItemId { get; set; }
    }

    public class RestaurantReviewUpdateViewModel : RestaurantReviewCreateViewModel
    {
        [Required(ErrorMessage = "ID is required")]
        [Display(Name = "ID")]
        public int Id { get; set; }
    }

    public class MenuItemReviewUpdateViewModel : MenuItemReviewCreateViewModel
    {
        [Required(ErrorMessage = "ID is required")]
        [Display(Name = "ID")]
        public int Id { get; set; }
    }
} 