using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Address
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Address line is required")]
        [StringLength(100, ErrorMessage = "Address line cannot exceed 100 characters")]
        [Display(Name = "Address Line")]
        public string AddressLine { get; set; } = string.Empty;

        
        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State name cannot exceed 50 characters")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country name cannot exceed 50 characters")]
        public string Country { get; set; } = string.Empty;

        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }

        [Display(Name = "Address Type")]
        public AddressType AddressType { get; set; }

        [Display(Name = "Additional Instructions")]
        [StringLength(200, ErrorMessage = "Additional instructions cannot exceed 200 characters")]
        public string? AdditionalInstructions { get; set; }

        public string FullAddress => $"{AddressLine}, {City}, {State} {PostalCode}, {Country}";
    }

    public class CreateAddressViewModel
    {
        [Required(ErrorMessage = "Address line is required")]
        [StringLength(100, ErrorMessage = "Address line cannot exceed 100 characters")]
        [Display(Name = "Address Line")]
        public string AddressLine { get; set; } = string.Empty;

        [Required(ErrorMessage = "City is required")]
        [StringLength(50, ErrorMessage = "City name cannot exceed 50 characters")]
        public string City { get; set; } = string.Empty;

        [Required(ErrorMessage = "State is required")]
        [StringLength(50, ErrorMessage = "State name cannot exceed 50 characters")]
        public string State { get; set; } = string.Empty;

        [Required(ErrorMessage = "Postal code is required")]
        [StringLength(10, ErrorMessage = "Postal code cannot exceed 10 characters")]
        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Country is required")]
        [StringLength(50, ErrorMessage = "Country name cannot exceed 50 characters")]
        public string Country { get; set; } = string.Empty;

        [Display(Name = "Is Default")]
        public bool IsDefault { get; set; }

        [Display(Name = "Address Type")]
        public AddressType AddressType { get; set; }

        [Display(Name = "Additional Instructions")]
        [StringLength(200, ErrorMessage = "Additional instructions cannot exceed 200 characters")]
        public string? AdditionalInstructions { get; set; }
    }

    public class EditAddressViewModel : CreateAddressViewModel
    {
        public int Id { get; set; }
    }

    public class AddressListViewModel
    {
        public List<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();
    }

    public enum AddressType
    {
        Home,
        Work,
        Other
    }
} 