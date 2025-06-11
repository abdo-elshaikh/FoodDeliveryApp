using System.Threading.Tasks;

namespace FoodDeliveryApp.Services.Interfaces
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendOrderConfirmationAsync(string email, string orderNumber, string orderDetails);
        Task SendOrderStatusUpdateAsync(string email, string orderNumber, string status, string message);
        Task SendOrderCancellationAsync(string email, string orderNumber, string reason);
    }
} 