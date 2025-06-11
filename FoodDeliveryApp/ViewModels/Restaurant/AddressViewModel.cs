using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Restaurant
{
    public class RestaurantAddressViewModel
    {
        [Display(Name = "Street Address")]
        public string Street { get; set; } = string.Empty;

        [Display(Name = "City")]
        public string City { get; set; } = string.Empty;

        [Display(Name = "State")]
        public string State { get; set; } = string.Empty;

        [Display(Name = "Country")]
        public string Country { get; set; } = "Egypt";

        [Display(Name = "Postal Code")]
        public string PostalCode { get; set; } = string.Empty;
    }
}
