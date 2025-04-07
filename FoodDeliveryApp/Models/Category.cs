using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required]
        public string CategoryName { get; set; }
        public string? CategoryDescription { get; set; }
        public byte[]? CategoryImage { get; set; }

        public virtual ICollection<Item> Items { get; set; }
    }
}
