using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace FoodDeliveryApp.ViewModels
{
    public abstract class ViewModelBase
    {
        public bool IsAuthenticated { get; set; }
        public string CurrentUserId { get; set; }
        public string CurrentUserName { get; set; }
        public string CurrentUserEmail { get; set; }
        public string CurrentUserProfilePicture { get; set; }
        public List<string> UserRoles { get; set; } = new List<string>();
        public Dictionary<string, string> Alerts { get; set; } = new Dictionary<string, string>();
        public ModelStateDictionary ValidationErrors { get; set; }
        public string ReturnUrl { get; set; }
        public bool IsMobile { get; set; }
        public string CurrentTheme { get; set; } = "light";
        public Dictionary<string, object> ViewData { get; set; } = new Dictionary<string, object>();

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

        public void SetViewData(string key, object value)
        {
            if (ViewData.ContainsKey(key))
            {
                ViewData[key] = value;
            }
            else
            {
                ViewData.Add(key, value);
            }
        }

        public T GetViewData<T>(string key, T defaultValue = default)
        {
            if (ViewData.TryGetValue(key, out object value) && value is T typedValue)
            {
                return typedValue;
            }
            return defaultValue;
        }
    }
}
