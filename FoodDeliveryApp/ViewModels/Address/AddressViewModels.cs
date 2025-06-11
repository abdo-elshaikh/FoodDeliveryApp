using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.ViewModels.Address
{
    public class AddressViewModel : BaseViewModel
    {

        [Required(ErrorMessage = "Street address is required")]
        [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State name cannot exceed 50 characters")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid postal code format")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
        public string Country { get; set; } = "United States";

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public new string? Notes { get; set; }

        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "Address type is required")]
        [Display(Name = "Address Type")]
        public Models.AddressType AddressType { get; set; }

        public string FormattedAddress => $"{StreetAddress}, {City}, {State} {PostalCode}";
    }

    public class AddressListViewModel
    {
        public List<AddressViewModel> Addresses { get; set; } = new();
        public bool HasDefaultAddress => Addresses.Any(a => a.IsDefault);
    }

    public class AddressCreateViewModel
    {
        [Required(ErrorMessage = "Street address is required")]
        [StringLength(200, ErrorMessage = "Street address cannot exceed 200 characters")]
        [Display(Name = "Street Address")]
        public string StreetAddress { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(100, ErrorMessage = "City name cannot exceed 100 characters")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State name cannot exceed 50 characters")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(20, ErrorMessage = "Postal code cannot exceed 20 characters")]
        [RegularExpression(@"^\d{5}(-\d{4})?$", ErrorMessage = "Invalid postal code format")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [StringLength(100, ErrorMessage = "Country name cannot exceed 100 characters")]
        public string Country { get; set; } = "United States";

        [StringLength(500, ErrorMessage = "Notes cannot exceed 500 characters")]
        public string? Notes { get; set; }

        public bool IsDefault { get; set; }

        [Required(ErrorMessage = "Address type is required")]
        [Display(Name = "Address Type")]
        public AddressType AddressType { get; set; }
    }

    public class AddressEditViewModel : AddressCreateViewModel
    {
        public int Id { get; set; }
    }
}
