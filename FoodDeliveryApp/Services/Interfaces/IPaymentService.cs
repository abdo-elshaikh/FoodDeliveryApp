using System.Threading.Tasks;
using FoodDeliveryApp.Models;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPaymentAsync(Order order);
        Task<PaymentResult> ProcessRefundAsync(Order order);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public string TransactionId { get; set; }
    }
} 