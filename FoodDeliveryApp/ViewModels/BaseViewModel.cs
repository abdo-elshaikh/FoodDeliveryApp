using System;
using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoodDeliveryApp.ViewModels
{
    /// <summary>
    /// Base view model that provides common properties and functionality for all view models
    /// </summary>
    public abstract class BaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [Display(Name = "Created At")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Is Active")]
        public bool IsActive { get; set; } = true;

        [Timestamp]
        public byte[]? Version { get; set; }

        [Display(Name = "Created By")]
        [StringLength(450)]
        public string? CreatedBy { get; set; }

        [Display(Name = "Updated By")]
        [StringLength(450)]
        public string? UpdatedBy { get; set; }

        [Display(Name = "Notes")]
        [StringLength(1000)]
        public string? Notes { get; set; }

        [Display(Name = "Custom Fields")]
        public Dictionary<string, string> CustomFields { get; set; } = new();
    }

    /// <summary>
    /// Base view model for list views with pagination, sorting, and filtering
    /// </summary>
    public abstract class BaseListViewModel<T>
    {
        public List<T> Items { get; set; } = new();

        [Range(0, int.MaxValue)]
        [Display(Name = "Total Count")]
        public int TotalCount { get; set; }

        [Range(1, int.MaxValue)]
        [Display(Name = "Current Page")]
        public int CurrentPage { get; set; } = 1;

        [Range(1, 100)]
        [Display(Name = "Page Size")]
        public int PageSize { get; set; } = 10;

        [Display(Name = "Total Pages")]
        public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);

        [StringLength(100)]
        [Display(Name = "Search Term")]
        public string? SearchTerm { get; set; }

        [StringLength(50)]
        [Display(Name = "Sort By")]
        public string? SortBy { get; set; }

        [StringLength(10)]
        [Display(Name = "Sort Order")]
        public string? SortOrder { get; set; }

        [Display(Name = "Include Inactive")]
        public bool IncludeInactive { get; set; }

        [Display(Name = "Date Range Start")]
        [DataType(DataType.Date)]
        public DateTime? DateRangeStart { get; set; }

        [Display(Name = "Date Range End")]
        [DataType(DataType.Date)]
        public DateTime? DateRangeEnd { get; set; }

        [Display(Name = "Has Previous Page")]
        public bool HasPreviousPage => CurrentPage > 1;

        [Display(Name = "Has Next Page")]
        public bool HasNextPage => CurrentPage < TotalPages;

        [Display(Name = "Validation Errors")]
        public ModelStateDictionary? ValidationErrors { get; set; }

        [Display(Name = "Alerts")]
        public Dictionary<string, string> Alerts { get; set; } = new();

        public void AddAlert(string type, string message)
        {
            if (Alerts.ContainsKey(type))
            {
                Alerts[type] = message;
            }
            else
            {
                Alerts.Add(type, message);
            }
        }

        public void AddSuccessAlert(string message) => AddAlert("success", message);
        public void AddErrorAlert(string message) => AddAlert("danger", message);
        public void AddWarningAlert(string message) => AddAlert("warning", message);
        public void AddInfoAlert(string message) => AddAlert("info", message);
    }

    /// <summary>
    /// Base view model for create operations
    /// </summary>
    public abstract class BaseCreateViewModel
    {
        [Display(Name = "Active")]
        public bool IsActive { get; set; } = true;

        [Display(Name = "Notes")]
        [StringLength(1000)]
        public string? Notes { get; set; }

        [Display(Name = "Custom Fields")]
        public Dictionary<string, string> CustomFields { get; set; } = new();

        [Display(Name = "Validation Errors")]
        public ModelStateDictionary? ValidationErrors { get; set; }

        [Display(Name = "Alerts")]
        public Dictionary<string, string> Alerts { get; set; } = new();

        public void AddAlert(string type, string message)
        {
            if (Alerts.ContainsKey(type))
            {
                Alerts[type] = message;
            }
            else
            {
                Alerts.Add(type, message);
            }
        }

        public void AddSuccessAlert(string message) => AddAlert("success", message);
        public void AddErrorAlert(string message) => AddAlert("danger", message);
        public void AddWarningAlert(string message) => AddAlert("warning", message);
        public void AddInfoAlert(string message) => AddAlert("info", message);
    }

    /// <summary>
    /// Base view model for edit operations
    /// </summary>
    public abstract class BaseEditViewModel : BaseCreateViewModel
    {
        [Required]
        [Display(Name = "ID")]
        public int Id { get; set; }

        [Timestamp]
        [Display(Name = "Version")]
        public byte[]? Version { get; set; }

        [Display(Name = "Created At")]
        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; }

        [Display(Name = "Updated At")]
        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        [Display(Name = "Created By")]
        [StringLength(450)]
        public string? CreatedBy { get; set; }

        [Display(Name = "Updated By")]
        [StringLength(450)]
        public string? UpdatedBy { get; set; }
    }
} 