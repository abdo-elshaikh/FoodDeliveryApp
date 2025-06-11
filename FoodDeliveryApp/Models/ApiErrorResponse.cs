namespace FoodDeliveryApp.Models
{
    public class ApiErrorResponse
    {
        public int StatusCode { get; set; }
        public string Message { get; set; }
        public string ErrorCode { get; set; }
        public string Details { get; set; }
    }
} 