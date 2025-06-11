using System.ComponentModel.DataAnnotations;

using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.Models
{
    public class UserLocation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string City { get; set; }

        [Required]
        [StringLength(100)]
        public string State { get; set; }

        [Required]
        [StringLength(100)]
        public string Country { get; set; }

        [Range(-90, 90)]
        public double Latitude { get; set; }

        [Range(-180, 180)]
        public double Longitude { get; set; }

        [StringLength(20)]
        public string PostalCode { get; set; }
    }
}
