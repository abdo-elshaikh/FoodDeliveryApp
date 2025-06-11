using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace FoodDeliveryApp.ViewModels.MenuItem
{
    public class MenuItemCategoryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MenuItemCount { get; set; }
        public string ImageUrl { get; set; }
        public List<MenuItemSummaryViewModel> MenuItems { get; set; } = new List<MenuItemSummaryViewModel>();
    }

    public class MenuItemCategoryCreateViewModel
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class MenuItemCategoryEditViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int MenuItemCount { get; set; }
        public string CurrentImageUrl { get; set; }
        public IFormFile ImageFile { get; set; }
    }

    public class MenuItemCategoryListViewModel
    {
        public List<MenuItemCategoryViewModel> Categories { get; set; }
        public string SearchTerm { get; set; }
        public string SortBy { get; set; }
        public string SortOrder { get; set; }
        public int PageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItems { get; set; }
    }

    public class MenuItemSummaryViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        public decimal Price { get; set; }
    }
}
