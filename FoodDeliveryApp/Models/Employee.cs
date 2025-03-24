using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Employee
    {
        [Key]
        public string EmpId { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }
        public decimal Salary { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }

        // Navigation property for Order
        public ICollection<Order> Orders { get; set; }
    }
}
