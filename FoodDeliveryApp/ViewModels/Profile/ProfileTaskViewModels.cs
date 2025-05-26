using FoodDeliveryApp.Models;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.ProfileViewModels
{
    // Address model for view
    public class AddressViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        
        [Required(ErrorMessage = "Address name is required")]
        [Display(Name = "Address Name (e.g. Home, Work)")]
        public string AddressName { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "Street address is required")]
        public string Street { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "City is required")]
        public string City { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "State is required")]
        public string State { get; set; } = string.Empty;
        
        [Required(ErrorMessage = "ZIP code is required")]
        [Display(Name = "ZIP Code")]
        public string ZipCode { get; set; } = string.Empty;
        
        [Display(Name = "Make Default Address")]
        public bool IsDefault { get; set; }
    }
    
    // Tasks view models
    public class EmployeeTasksViewModel
    {
        public int EmployeeId { get; set; }
        public string EmployeeName { get; set; } = string.Empty;
        public List<EmployeeTaskViewModel> Tasks { get; set; } = new();
    }
    
    public class EmployeeTaskViewModel
    {
        public int OrderId { get; set; }
        public string OrderNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public OrderStatus Status { get; set; }
        public DateTime AssignedDate { get; set; }
        public string DeliveryAddress { get; set; } = string.Empty;
        public DateTime? EstimatedDeliveryTime { get; set; }
    }
} 