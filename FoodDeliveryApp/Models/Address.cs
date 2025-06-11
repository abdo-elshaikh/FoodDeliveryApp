using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FoodDeliveryApp.Models
{
    public class Address
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string City { get; set; } = string.Empty;

        [Required]
        [StringLength(50)]
        public string State { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string Country { get; set; } = "United States";

        [StringLength(500)]
        public string? Notes { get; set; }

        [Required]
        [StringLength(450)]
        public string UserId { get; set; } = string.Empty;

        public bool IsDefault { get; set; }

        [Display(Name = "Address Type")]
        public AddressType AddressType { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public double? Latitude { get; set; }

        public double? Longitude { get; set; }

        public virtual ApplicationUser User { get; set; } = null!;

        [NotMapped]
        public string FullAddress => $"{StreetAddress}, {City}, {State} {PostalCode}, {Country}";
    }

    public enum AddressType
    {
        Home,
        Work,
        Other
    }
}
