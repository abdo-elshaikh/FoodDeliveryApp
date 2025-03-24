using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class OrderDetail
    {
        [Key]
        public int OrdDetId { get; set; }
        public int OrdId { get; set; }
        public int ItemId { get; set; }
        public int OrdQuantity { get; set; }
        public decimal OrdDiscount { get; set; }
        public decimal OrdTax { get; set; }
        public decimal OrdAmount { get; set; }
        [ForeignKey("OrdId")]
        public Order Order { get; set; }
        [ForeignKey("ItemId")]
        public Item Item { get; set; }
    }
}
