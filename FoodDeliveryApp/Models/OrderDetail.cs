using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrdDetId { get; set; }
        public int OrdQuantity { get; set; }
        public decimal OrdDiscount { get; set; }
        public decimal OrdTax { get; set; }
        public decimal OrdAmount { get; set; }
        public decimal OrdTotal { get; set; }
        public int OrdId { get; set; }
        public int ItemId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        [ForeignKey("OrdId")]
        public virtual Order Order { get; set; }
        [ForeignKey("ItemId")]
        public virtual Item Item { get; set; }
    }
}
