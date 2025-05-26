using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.Models;
using Microsoft.AspNetCore.Http;

namespace FoodDeliveryApp.ViewModels.MenuItem
{
    public class MenuItemViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }
        public string RestaurantName { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool IsSpicy { get; set; }

        [Display(Name = "Preparation Time")]
        [Range(0, 120, ErrorMessage = "Preparation time must be between 0 and 120 minutes")]
        public int PreparationTime { get; set; }

        public double Rating { get; set; }
        public int TotalReviews { get; set; }
        public List<MenuItemCustomizationViewModel> Customizations { get; set; } = new();
    }

    public class CreateMenuItemViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Description is required")]
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string Description { get; set; } = string.Empty;

        [Required(ErrorMessage = "Price is required")]
        [Range(0.01, 1000, ErrorMessage = "Price must be between 0.01 and 1000")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        [Display(Name = "Image")]
        public IFormFile? Image { get; set; }

        [Required(ErrorMessage = "Category is required")]
        [Display(Name = "Category")]
        public int CategoryId { get; set; }

        [Required(ErrorMessage = "Restaurant is required")]
        [Display(Name = "Restaurant")]
        public int RestaurantId { get; set; }

        public bool IsAvailable { get; set; } = true;
        public bool IsVegetarian { get; set; }
        public bool IsVegan { get; set; }
        public bool IsGlutenFree { get; set; }
        public bool IsSpicy { get; set; }

        [Display(Name = "Preparation Time")]
        [Range(0, 120, ErrorMessage = "Preparation time must be between 0 and 120 minutes")]
        public int PreparationTime { get; set; }

        public List<MenuItemCustomizationViewModel> Customizations { get; set; } = new();
    }

    public class EditMenuItemViewModel : CreateMenuItemViewModel
    {
        public int Id { get; set; }
        public string? CurrentImageUrl { get; set; }
    }

    public class MenuItemCustomizationViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "Type is required")]
        [Display(Name = "Type")]
        public CustomizationType Type { get; set; }

        public bool IsRequired { get; set; }
        public int? MinSelections { get; set; }
        public int? MaxSelections { get; set; }
        public List<CustomizationOptionViewModel> Options { get; set; } = new();
    }

    public class CustomizationOptionViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [Range(0, 100, ErrorMessage = "Price must be between 0 and 100")]
        [DataType(DataType.Currency)]
        public decimal Price { get; set; }

        public bool IsDefault { get; set; }
    }

    public enum CustomizationType
    {
        Single,
        Multiple,
        Text
    }
} 