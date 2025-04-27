using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        [Display(Name = "Address Title (e.g., Home, Work)")]
        public string Title { get; set; } = "Home";

        [Required]
        [StringLength(100)]
        [Display(Name = "Street Address")]
        public string Street { get; set; }

        [Required]
        [StringLength(50)]
        public string City { get; set; }

        [Required]
        [StringLength(50)]
        public string State { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; }

        [Required]
        [StringLength(50)]
        public string Country { get; set; } = "USA";

        [Display(Name = "Default Address")]
        public bool IsDefault { get; set; } = false;
        //FullAddress
        public string FullAddress => $"{Street}, {City}, {State}, {PostalCode}, {Country}";
    }
}
