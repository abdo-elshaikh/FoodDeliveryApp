using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.ViewModels.Address;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDeliveryApp.ViewModels.Restaurant
{
    public class RestaurantEditViewModel
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        [Display(Name = "Restaurant Name")]
        public string Name { get; set; }

        [StringLength(500)]
        [Display(Name = "Description")]
        public string Description { get; set; }

        [Required]
        [Phone]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Current Image")]
        public string CurrentImageUrl { get; set; }

        [Display(Name = "New Image")]
        public IFormFile? NewImageFile { get; set; } = null!;

        [Url]
        [Display(Name = "Website")]
        public string Website { get; set; }

        // location url (google maps)
        [Url]
        [Display(Name = "Location")]
        public string LocationUrl { get; set; } // google maps url

        [Required]
        [Range(0, double.MaxValue)]
        [Display(Name = "Delivery Fee")]
        public decimal DeliveryFee { get; set; } = 5; // 5$ delivery fee
        // tax rate
        [Required]
        [Range(0, 30)]
        [Display(Name = "Tax Rate (%)")]
        public decimal TaxRate { get; set; } = 10; // 10% tax rate

        [Required]
        [Display(Name = "Address")]
        public RestaurantAddressViewModel Address { get; set; } = new RestaurantAddressViewModel();

        [Required]
        [Display(Name = "Opening Time")]
        public TimeSpan OpeningTime { get; set; }

        [Required]
        [Display(Name = "Closing Time")]
        public TimeSpan ClosingTime { get; set; }

        [Required]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Display(Name = "Active")]
        public bool IsActive { get; set; }

        // estmated time to complete
        [Display(Name = "Estimated Time")]
        public string EstimatedTime { get; set; } = "30 - 40 minutes";

        public List<SelectListItem> Categories { get; set; } = new List<SelectListItem>();
    }
} 