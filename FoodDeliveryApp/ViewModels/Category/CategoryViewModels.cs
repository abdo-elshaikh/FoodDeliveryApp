using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FoodDeliveryApp.ViewModels.Category
{
    public class CategoryListViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new List<CategoryViewModel>();
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }

    public class CategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RestaurantCount { get; set; }
        public string ImageUrl { get; set; }
        public List<RestaurantSummaryViewModel> Restaurants { get; set; } = new List<RestaurantSummaryViewModel>();
    }

    public class CategoryCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class CategoryEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string CurrentImageUrl { get; set; }
        public IFormFile NewImageFile { get; set; }
        public int RestaurantCount { get; set; }
    }

    public class RestaurantSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public string Description { get; set; }
    }
}
