using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels
{
    public class ReviewViewModel
    {
        public int Id { get; set; }
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; }
        public string CustomerName { get; set; }
        public string CustomerId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        [StringLength(1000)]
        public string Comment { get; set; }

        public DateTime CreatedAt { get; set; }
    }

    public class CreateReviewViewModel
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public int RestaurantId { get; set; }

        [Required]
        [Range(1, 5)]
        public int Rating { get; set; } = 5;

        [StringLength(1000)]
        public string Comment { get; set; }
    }
}
