using Braintree;

namespace FoodDeliveryApp.Services
{
    public class BraintreeService : IBraintreeService
    {
        private readonly IConfiguration _config;
        private readonly IBraintreeGateway _gateway;

        public BraintreeService(IConfiguration config)
        {
            _config = config;
            _gateway = CreateGateway(); // Initialize gateway once
        }

        public IBraintreeGateway CreateGateway()
        {
            // Use proper environment based on configuration
            var environment = _config.GetValue<bool>("BraintreeGateway:UseProduction")
                ? Braintree.Environment.PRODUCTION
                : Braintree.Environment.SANDBOX;

            return new BraintreeGateway
            {
                Environment = environment,
                MerchantId = _config["BraintreeGateway:MerchantId"],
                PublicKey = _config["BraintreeGateway:PublicKey"],
                PrivateKey = _config["BraintreeGateway:PrivateKey"]
            };
        }

        public IBraintreeGateway GetGateway()
        {
            return _gateway; // Return cached instance
        }

        public string GenerateClientToken(string customerId = null)
        {
            var request = new ClientTokenRequest();

            if (!string.IsNullOrEmpty(customerId))
            {
                request.CustomerId = customerId;
            }

            return _gateway.ClientToken.Generate(request);
        }

        public Result<Transaction> MakeTransaction(decimal amount, string paymentMethodNonce,
            string merchantAccountId = null)
        {
            var request = new TransactionRequest
            {
                Amount = amount,
                PaymentMethodNonce = paymentMethodNonce,
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            if (!string.IsNullOrEmpty(merchantAccountId))
            {
                request.MerchantAccountId = merchantAccountId;
            }

            return _gateway.Transaction.Sale(request);
        }

        public Result<Customer> CreateCustomer(string firstName, string lastName, string email)
        {
            var request = new CustomerRequest
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email
            };

            return _gateway.Customer.Create(request);
        }
    }
}