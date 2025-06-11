using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FoodDeliveryApp.ViewModels
{
    /// <summary>
    /// Base view model class that provides common functionality for all view models
    /// </summary>
    public abstract class ViewModelBase
    {
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [Display(Name = "Description")]
        public string? Description { get; set; }

        [Display(Name = "Is Authenticated")]
        public bool IsAuthenticated { get; set; }

        [Display(Name = "User Name")]
        public string? UserName { get; set; }

        [Display(Name = "User Roles")]
        public List<string> UserRoles { get; set; } = new();

        [Display(Name = "Alerts")]
        public List<AlertViewModel> Alerts { get; set; } = new();

        [Display(Name = "Validation Errors")]
        public ModelStateDictionary? ValidationErrors { get; set; }

        [Display(Name = "View Data")]
        public Dictionary<string, object> ViewData { get; set; } = new();

        [Display(Name = "Select Lists")]
        public Dictionary<string, IEnumerable<SelectListItem>> SelectLists { get; set; } = new();

        [Display(Name = "Breadcrumbs")]
        public List<BreadcrumbItem> Breadcrumbs { get; set; } = new();

        [Display(Name = "Page Meta")]
        public PageMeta PageMeta { get; set; } = new();

        public void AddAlert(string message, AlertType type = AlertType.Info)
        {
            Alerts.Add(new AlertViewModel { Message = message, Type = type });
        }

        public void AddSuccessAlert(string message)
        {
            AddAlert(message, AlertType.Success);
        }

        public void AddErrorAlert(string message)
        {
            AddAlert(message, AlertType.Danger);
        }

        public void AddWarningAlert(string message)
        {
            AddAlert(message, AlertType.Warning);
        }

        public void AddInfoAlert(string message)
        {
            AddAlert(message, AlertType.Info);
        }

        public void AddBreadcrumb(string text, string? url = null)
        {
            Breadcrumbs.Add(new BreadcrumbItem { Text = text, Url = url });
        }

        public void AddSelectList(string key, IEnumerable<SelectListItem> items)
        {
            SelectLists[key] = items;
        }

        public void AddViewData(string key, object value)
        {
            ViewData[key] = value;
        }
    }

    /// <summary>
    /// Represents a breadcrumb item in the navigation
    /// </summary>
    public class BreadcrumbItem
    {
        [Required]
        [StringLength(100)]
        [Display(Name = "Text")]
        public string Text { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "URL")]
        public string? Url { get; set; }
    }

    /// <summary>
    /// Represents metadata for a page
    /// </summary>
    public class PageMeta
    {
        [StringLength(100)]
        [Display(Name = "Title")]
        public string Title { get; set; } = string.Empty;

        [StringLength(200)]
        [Display(Name = "Description")]
        public string? Description { get; set; }

        [StringLength(100)]
        [Display(Name = "Keywords")]
        public string? Keywords { get; set; }

        [StringLength(200)]
        [Display(Name = "Author")]
        public string? Author { get; set; }

        [StringLength(200)]
        [Display(Name = "Canonical URL")]
        public string? CanonicalUrl { get; set; }
    }

    /// <summary>
    /// Represents an alert message
    /// </summary>
    public class AlertViewModel
    {
        [Required]
        [StringLength(500)]
        [Display(Name = "Message")]
        public string Message { get; set; } = string.Empty;

        [Display(Name = "Type")]
        public AlertType Type { get; set; }

        [Display(Name = "Dismissible")]
        public bool Dismissible { get; set; } = true;
    }

    /// <summary>
    /// Represents the type of alert
    /// </summary>
    public enum AlertType
    {
        Success,
        Info,
        Warning,
        Danger
    }
}
