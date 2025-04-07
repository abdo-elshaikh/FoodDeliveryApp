using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ItemId { get; set; }
        public string ItemName { get; set; }
        public string? ItemDescription { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal ItemPrice { get; set; }
        public byte[]? ItemImage { get; set; }
        public bool IsAvailable { get; set; }
        public int CateqId { get; set; }
        [ForeignKey("CateqId")]
        public Category Category { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
