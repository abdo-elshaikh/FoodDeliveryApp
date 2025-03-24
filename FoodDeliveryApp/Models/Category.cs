using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class Category
    {
        [Key]
        public int CateqId { get; set; }
        public string CateqName { get; set; }
        public string CateqDescription { get; set; }
        public byte[] CateqImage { get; set; }
        public ICollection<Item> Items { get; set; }
    }
}
