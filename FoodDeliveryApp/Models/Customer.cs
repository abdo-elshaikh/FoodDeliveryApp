using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Customer
    {
        [Key]
        public string CustId { get; set; }
        public string Name { get; set; }
        public string City { get; set; }
        public string Address { get; set; }
        public string Phone { get; set; }

        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public User User { get; set; }
        // Navigation property for Order
        public ICollection<Order> Orders { get; set; }
    }
}
