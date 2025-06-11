﻿using FoodDeliveryApp.Models;
using FoodDeliveryApp.Services.Interfaces;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FoodDeliveryApp.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailSender> _logger;

        public EmailSender(
            IOptions<EmailSettings> emailSettings,
            ILogger<EmailSender> logger)
        {
            _emailSettings = emailSettings.Value ?? throw new ArgumentNullException(nameof(emailSettings));
            _logger = logger;

            ValidateEmailSettings(_emailSettings);
        }

        private void ValidateEmailSettings(EmailSettings settings)
        {
            if (string.IsNullOrWhiteSpace(settings.SmtpServer))
                throw new ArgumentException("SMTP server is not configured.");
            if (settings.SmtpPort <= 0)
                throw new ArgumentException("SMTP port is not configured or invalid.");
            if (string.IsNullOrWhiteSpace(settings.SenderEmail))
                throw new ArgumentException("Sender email is not configured.");
            if (string.IsNullOrWhiteSpace(settings.Username))
                throw new ArgumentException("SMTP username is not configured.");
            if (string.IsNullOrWhiteSpace(settings.Password))
                throw new ArgumentException("SMTP password is not configured.");
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Recipient email is required.", nameof(email));
            if (string.IsNullOrWhiteSpace(subject))
                throw new ArgumentException("Subject is required.", nameof(subject));
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException("Message body is required.", nameof(message));

            try
            {
                using var client = new SmtpClient(_emailSettings.SmtpServer, _emailSettings.SmtpPort)
                {
                    EnableSsl = _emailSettings.EnableSsl,
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
                _logger.LogError(ex, "Error sending email to {Recipient}", email);
                throw new InvalidOperationException($"Error sending email: {ex.Message}", ex);
            }
        }

        public async Task SendOrderConfirmationAsync(string email, string orderNumber, string orderDetails)
        {
            // TODO: Implement actual order confirmation email logic
            await SendEmailAsync(email, $"Order Confirmation - {orderNumber}", orderDetails);
        }

        public async Task SendOrderStatusUpdateAsync(string email, string orderNumber, string status, string message)
        {
            // TODO: Implement actual order status update email logic
            await SendEmailAsync(email, $"Order Status Update - {orderNumber}", $"Status: {status}<br/>{message}");
        }

        public async Task SendOrderCancellationAsync(string email, string orderNumber, string reason)
        {
            // TODO: Implement actual order cancellation email logic
            await SendEmailAsync(email, $"Order Cancelled - {orderNumber}", $"Reason: {reason}");
        }
    }
}
