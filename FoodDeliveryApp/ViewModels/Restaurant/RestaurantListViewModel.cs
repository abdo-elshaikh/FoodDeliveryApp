using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace FoodDeliveryApp.ViewModels.Restaurant
{
    public class RestaurantListViewModel
    {
        public IEnumerable<RestaurantViewModel> Restaurants { get; set; } = new List<RestaurantViewModel>();
        
        [Display(Name = "Page Number")]
        public int PageNumber { get; set; } = 1;
        
        [Display(Name = "Page Size")]
        public int PageSize { get; set; } = 12;
        
        [Display(Name = "Total Pages")]
        public int TotalPages { get; set; }

        // current page of the restaurant list
        [Display(Name = "Current Page")]
        public int CurrentPage { get; set; } = 1;

        
        [Display(Name = "Total Items")]
        public int TotalItems { get; set; }
        
        [Display(Name = "Search Term")]
        public string SearchTerm { get; set; } = string.Empty;
        
        [Display(Name = "Category")]
        public int? CategoryId { get; set; }
        
        [Display(Name = "Sort By")]
        public string SortBy { get; set; } = "name";
        // PriceRange
        public decimal PriceRange { get; set; } = decimal.MinValue;
    }
} 