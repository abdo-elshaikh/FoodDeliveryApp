using System.ComponentModel.DataAnnotations;
using FoodDeliveryApp.ViewModels.Restaurant;
using Microsoft.AspNetCore.Http;

namespace FoodDeliveryApp.ViewModels.Category
{
    public class CategoryViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        [Display(Name = "Image URL")]
        public string? ImageUrl { get; set; }

        public bool IsActive { get; set; }
        public int MenuItemCount { get; set; }
        public int RestaurantCount { get; set; }
    }

    public class CategoryCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "Description cannot exceed 200 characters")]
        public string? Description { get; set; }

        [Display(Name = "Image")]
        public IFormFile? ImageFile { get; set; }

        public bool IsActive { get; set; } = true;
    }

    public class CategoryEditViewModel : CategoryCreateViewModel
    {
        public int Id { get; set; }
        public string? CurrentImageUrl { get; set; }
        public IFormFile? NewImageFile { get; set; }
        public int RestaurantCount { get; set; }
    }

    public class CategoryListViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new();
        public int TotalCount { get; set; }
        // search term
        public string? SearchTerm { get; set; }
        // list of restaurants in this category
        public List<RestaurantViewModel> Restaurants { get; set; } = new();
    }
} 