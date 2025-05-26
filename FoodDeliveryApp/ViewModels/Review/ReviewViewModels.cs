using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;

namespace FoodDeliveryApp.ViewModels.Review
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string? CustomerImageUrl { get; set; }
        public int Rating { get; set; }
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;
        public int? MenuItemId { get; set; }
        public string? MenuItemName { get; set; }
        public bool IsVerified { get; set; }
        public bool IsHelpful { get; set; }
        public int HelpfulCount { get; set; }
    }

    public class CreateReviewViewModel
    {
        [Required(ErrorMessage = "Rating is required")]
        [Range(1, 5, ErrorMessage = "Rating must be between 1 and 5")]
        public int Rating { get; set; }

        [StringLength(500, ErrorMessage = "Comment cannot exceed 500 characters")]
        public string? Comment { get; set; }

        public int RestaurantId { get; set; }
        public int? MenuItemId { get; set; }
    }

    public class EditReviewViewModel : CreateReviewViewModel
    {
        public int Id { get; set; }
    }

    public class ReviewListViewModel
    {
        public List<ReviewViewModel> Reviews { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }

    public class ReviewSummaryViewModel
    {
        public double AverageRating { get; set; }
        public int TotalReviews { get; set; }
        public Dictionary<int, int> RatingDistribution { get; set; } = new();
        public List<ReviewListViewModel> RecentReviews { get; set; } = new();
    }
} 