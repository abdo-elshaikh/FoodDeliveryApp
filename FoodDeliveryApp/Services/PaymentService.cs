using FoodDeliveryApp.Repositories.Interfaces;

namespace FoodDeliveryApp.Services
{
    public class PaymentService : IPaymentService
    {
        //private readonly IOptions<StripeSettings> _stripeSettings;
        private readonly IOrderRepository _orderRepository;

        public PaymentService(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
            //var stripeSettings = _stripeSettings.Value;
            //StripeConfiguration.ApiKey = stripeSettings.SecretKey;
        }
        public Task<PaymentResult> ProcessPayment(int paymentMethodId, decimal amount, string description)
        {
            throw new NotImplementedException();
        }

        public Task<RefundResult> ProcessRefund(string transactionId, decimal amount, string reason)
        {
            throw new NotImplementedException();
        }
    }
}