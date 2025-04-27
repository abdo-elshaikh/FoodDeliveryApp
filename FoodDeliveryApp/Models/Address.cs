using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Address
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; } = "Home";

        [Required]
        [StringLength(100)]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string? City { get; set; }

        [Required]
        [StringLength(50)]
        public string? State { get; set; }

        [Required]
        [StringLength(20)]
        public string? PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string? Country { get; set; } = "USA";

        public bool IsDefault { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        [Required]
        public int CustomerProfileId { get; set; }

        [ForeignKey("CustomerProfileId")]
        public virtual CustomerProfile CustomerProfile { get; set; }
    }
}
