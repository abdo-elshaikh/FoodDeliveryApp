using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class RestaurantCategory : BaseEntity
    {

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(255)]
        public string? Description { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public virtual ICollection<Restaurant> Restaurants { get; set; } = new HashSet<Restaurant>();
        public virtual ICollection<MenuItem> MenuItems { get; set; } = new HashSet<MenuItem>();
    }
}
