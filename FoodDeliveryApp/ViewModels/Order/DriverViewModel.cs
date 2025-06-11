using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Order
{
    public class DriverViewModel
    {
        public int DriverId { get; set; }
        
        [Display(Name = "Name")]
        public string Name { get; set; }
        
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }
        
        [Display(Name = "Profile Picture")]
        public string ProfilePictureUrl { get; set; }
        
        [Display(Name = "Vehicle Type")]
        public string VehicleType { get; set; }
        
        [Display(Name = "License Plate")]
        public string LicensePlate { get; set; }
    }
} 