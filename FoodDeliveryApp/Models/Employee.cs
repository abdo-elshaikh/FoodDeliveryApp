using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public enum EmployeePosition
    {
        Manager,
        DeliveryDriver,
        Chef,
        CustomerService,
        Cashier
    }

    public class EmployeeProfile
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [Required]
        [StringLength(50)]
        public string LastName { get; set; }

        [Phone]
        [StringLength(20)]
        public string? PhoneNumber { get; set; }

        [Required]
        public EmployeePosition Position { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime HireDate { get; set; } = DateTime.UtcNow;

        [DataType(DataType.Date)]
        public DateTime? TerminationDate { get; set; }

        [StringLength(255)]
        public string? ProfilePictureUrl { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation properties
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Order> DeliveryOrders { get; set; } = new HashSet<Order>();
    }
}
