using Braintree;

namespace FoodDeliveryApp.Services
{
    public interface IBraintreeService
    {
        IBraintreeGateway CreateGateway();
        IBraintreeGateway GetGateway();
        string GenerateClientToken(string customerId = null);
        Result<Transaction> MakeTransaction(decimal amount, string paymentMethodNonce,
            string merchantAccountId = null);
        Result<Customer> CreateCustomer(string firstName, string lastName, string email);
    }
}
