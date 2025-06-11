using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class MenuItem : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        [Required]
        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        [StringLength(255)]
        public string? ImageUrl { get; set; }

        public bool IsAvailable { get; set; } = true;

        [Required]
        public int RestaurantId { get; set; }

        public int? CategoryId { get; set; }

        public double Rating { get; set; } = 0;

        public TimeSpan PreparationTime { get; set; } = TimeSpan.Zero;

        public int Calories { get; set; } = 0;

        public int SpiceLevel { get; set; } = 0;

        public bool IsVegetarian { get; set; } = false;

        public bool IsVegan { get; set; } = false;

        // tags
        [NotMapped]
        public virtual ICollection<string> Tags { get; set; } = new HashSet<string>();

        // Navigation properties
        public virtual MenuItemCategory Category { get; set; } = null!;
        public virtual Restaurant Restaurant { get; set; } = null!;
        public virtual ICollection<OrderItem> OrderItems { get; set; } = new HashSet<OrderItem>();
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();


        // help methods
        public bool IsSpicy()
        {
            return SpiceLevel > 5;
        }

        public bool IsHealthy()
        {
            return IsSpicy() == false && IsVegetarian == false && IsVegan == false;
        }

        public bool IsPopular()
        {
            return Rating > 4.5;
        }

        public bool IsNew()
        {
            return CreatedAt > DateTime.UtcNow.AddDays(-30);
        }

        public bool IsOnSale()
        {
            return Price < 10;
        }

        public void AddTag(string tag)
        {
            Tags.Add(tag);
        }

        public void RemoveTag(string tag)
        {
            Tags.Remove(tag);
        }

        public void UpdateTags(string[] newTags)
        {
            Tags.Clear();
            AddTags(newTags);
        }

        private void AddTags(string[] newTags)
        {
            foreach (var tag in newTags)
            {
                AddTag(tag);
            }
        }
        
        public void UpdateRating(double newRating)
        {
            Rating = (Rating * Reviews.Count + newRating) / (Reviews.Count + 1);
        }
    }
    
}
