using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDeliveryApp.ViewModels
{
    public class CategoryViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
        
        public string? ImageUrl { get; set; }
        
        public int? ParentCategoryId { get; set; }
        
        [Display(Name = "Parent Category")]
        public string? ParentCategoryName { get; set; }
        
        public int ItemCount { get; set; }
    }
    
    public class CategoryListViewModel
    {
        public List<CategoryViewModel> Categories { get; set; } = new();
        public int TotalCount { get; set; }
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
        public string? SearchTerm { get; set; }
        public string? SortBy { get; set; }
        public string? SortOrder { get; set; }
    }
    
    public class CategoryCreateViewModel
    {
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;
        
        public IFormFile? Image { get; set; }
        
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }
        
        public List<SelectListItem>? ParentCategoryOptions { get; set; }
    }
    
    public class CategoryEditViewModel
    {
        public int Id { get; set; }
        
        [Required(ErrorMessage = "Name is required")]
        [StringLength(100, ErrorMessage = "Name cannot exceed 100 characters")]
        [Display(Name = "Category Name")]
        public string Name { get; set; } = string.Empty;
        
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
        public string? Description { get; set; }
        
        [Display(Name = "Display Order")]
        public int DisplayOrder { get; set; }
        
        [Display(Name = "Is Active")]
        public bool IsActive { get; set; }
        
        public IFormFile? Image { get; set; }
        
        public string? ExistingImageUrl { get; set; }
        
        [Display(Name = "Parent Category")]
        public int? ParentCategoryId { get; set; }
        
        public List<SelectListItem>? ParentCategoryOptions { get; set; }
    }
} 