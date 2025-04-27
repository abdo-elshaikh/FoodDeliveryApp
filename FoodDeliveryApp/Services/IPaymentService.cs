namespace FoodDeliveryApp.Services
{
    public interface IPaymentService
    {
        Task<PaymentResult> ProcessPayment(int paymentMethodId, decimal amount, string description);
        Task<RefundResult> ProcessRefund(string transactionId, decimal amount, string reason);
    }

    public class PaymentResult
    {
        public bool Success { get; set; }
        public string TransactionId { get; set; }
        public string ErrorMessage { get; set; }
    }

    public class RefundResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
    }
}
