using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.ViewModels.Address;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDeliveryApp.ViewModels.Restaurant
{
    public class RestaurantCreateViewModel
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; } = string.Empty;

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; } = "";

        [Display(Name = "Restaurant Image")]
        public IFormFile? ImageFile { get; set; } = null!;

        [Url]
        [Display(Name = "Website")]
        public string Website { get; set; } = "";
        [Url]
        [Display(Name = "Location")]
        public string LocationUrl { get; set; } = "";

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Delivery Fee")]
        public decimal DeliveryFee { get; set; } = 5;

        [Required]
        [Range(0, 30)]
        [Display(Name = "Tax Rate")]
        public int TaxRate { get; set; } = 10;

        [Required]
        [Display(Name = "Address")]
        public RestaurantAddressViewModel Address { get; set; } = new RestaurantAddressViewModel();

        [Required]
        [Display(Name = "Opening Time")]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }

        // DeliveryTime
        [Required]
        [Display(Name = "Delivery Time (minutes)")]
        public string DeliveryTime { get; set; } = "30 - 45 minutes";

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }


}