using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using FoodDeliveryApp.ViewModels.Address;

namespace FoodDeliveryApp.ViewModels.Account
{
    public class ProfileViewModel
    {
        public string Id { get; set; }

        [Required]
        [Display(Name = "Username")]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Phone]
        [Display(Name = "Phone number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Profile Picture")]
        public string ProfilePictureUrl { get; set; }

        [Display(Name = "First Name")]
        public string FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Display(Name = "Date of Birth")]
        [DataType(DataType.Date)]
        public DateTime? DateOfBirth { get; set; }

        public List<AddressViewModel> Addresses { get; set; } = new List<AddressViewModel>();
        public List<OrderHistoryViewModel> OrderHistory { get; set; } = new List<OrderHistoryViewModel>();
        public List<FavoriteViewModel> Favorites { get; set; } = new List<FavoriteViewModel>();
    }



    public class OrderHistoryViewModel
    {
        public int Id { get; set; }
        public string OrderNumber { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }
        public string RestaurantName { get; set; }
        public List<OrderItemViewModel> Items { get; set; } = new List<OrderItemViewModel>();

        // GetStatusBadgeClass returns a CSS class based on the status
        public string GetStatusBadgeClass()
        {
            return Status switch
            {
                OrderStatus.Pending => "badge badge-warning",
                OrderStatus.Confirmed => "badge badge-info",
                OrderStatus.InPreparation => "badge badge-primary",
                OrderStatus.ReadyForPickup => "badge badge-secondary",
                OrderStatus.OutForDelivery => "badge badge-info",
                OrderStatus.Delivered => "badge badge-success",
                OrderStatus.Canceled => "badge badge-danger",
                _ => "badge badge-secondary"
            };
        }
    }

    public class OrderItemViewModel
    {
        public int Id { get; set; }
        public string ItemName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string SpecialInstructions { get; set; }
    }

    public class FavoriteViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Type { get; set; } // Restaurant or MenuItem
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
} 