using System;

namespace FoodDeliveryApp.ViewModels
{
    public class ErrorViewModel
    {
        public string? RequestId { get; set; }
        public bool ShowRequestId { get; set; }
        public Exception? Exception { get; set; }
        public int StatusCode { get; set; }
        public string? ErrorMessage { get; set; }
        public string? ErrorTitle { get; set; }
        public bool IsDevelopment { get; set; }
    }
} 