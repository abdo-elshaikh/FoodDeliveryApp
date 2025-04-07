using FoodDeliveryApp.Models;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FoodDeliveryApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;

        public EmailSender(IOptions<EmailSettings> emailSettings)
        {
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(_emailSettings.Username, _emailSettings.Password)
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_emailSettings.SenderEmail, _emailSettings.SenderName),
                    Subject = subject,
                    Body = message,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(email);

                await client.SendMailAsync(mailMessage);
            }
            catch (Exception ex)
            {
                // Log the exception (you might want to use ILogger here)
                throw new InvalidOperationException($"Error sending email: {ex.Message}", ex);
            }
        }

        public Task SendEmailWithAttachmentAsync(string email, string subject, string message, string attachmentPath)
        {
            throw new NotImplementedException();
        }

        public Task SendEmailWithTemplateAsync(string email, string subject, string templatePath, object model)
        {
            throw new NotImplementedException();
        }
    }
}