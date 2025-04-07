namespace FoodDeliveryApp.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task SendEmailWithAttachmentAsync(string email, string subject, string message, string attachmentPath);
        Task SendEmailWithTemplateAsync(string email, string subject, string templatePath, object model);
    }
}