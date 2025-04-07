using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public enum OrderStatus
    {
        Pending,
        InProgress,
        Completed,
        Cancelled
    }
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrdId { get; set; }
        [Required(ErrorMessage = "Order Date is required.")]
        [DataType(DataType.Date)]
        public DateTime OrdDate { get; set; }
        [Column(TypeName = "decimal(18, 2)")]
        public decimal OrdSum { get; set; }
        [Required(ErrorMessage = "Order Status is required.")]
        public OrderStatus OrdStatus { get; set; }
        public int EmployeeId { get; set; }
        public int CustomerId { get; set; }
        [ForeignKey("EmployeeId")]
        public Employee Employee { get; set; }
        [ForeignKey("CustomerId")]
        public Customer Customer { get; set; }
        // Navigation property for OrderDetail
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}