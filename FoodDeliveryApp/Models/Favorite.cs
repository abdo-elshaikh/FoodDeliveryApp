using System;

namespace FoodDeliveryApp.Models
{
    public class Favorite : BaseEntity
    {
        public string UserId { get; set; }
        public int MenuItemId { get; set; }

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual MenuItem MenuItem { get; set; }
    }
}
