using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Order
    {
        [Key]
        public int OrdId { get; set; }
        [Required(ErrorMessage = "Order Date is required.")]
        [DataType(DataType.Date)]
        public DateTime OrdDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OrdSum { get; set; }
        [Required(ErrorMessage = "Order Status is required.")]
        public OrderStatus OrdStatus { get; set; }
        public string EmpId { get; set; }
        public string CustId { get; set; }
        [ForeignKey("EmpId")]
        public Employee Employee { get; set; }
        [ForeignKey("CustId")]
        public Customer Customer { get; set; }
        // Navigation property for OrderDetail
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
