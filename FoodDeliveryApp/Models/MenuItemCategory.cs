using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class MenuItemCategory : BaseEntity
    {
        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(255)]
        public string ImageUrl { get; set; } = string.Empty;

        [StringLength(500)]
        public string Description { get; set; } = string.Empty;

        public virtual ICollection<MenuItem> MenuItems { get; set; } = new HashSet<MenuItem>();

        [NotMapped]
        public int MenuItemCount => MenuItems.Count;
        
    }
}
