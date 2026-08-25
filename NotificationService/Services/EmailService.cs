using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
using Microsoft.Extensions.Configuration;

namespace NotificationService.Services
{
    public class EmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmployeeCreatedEmailAsync(
            string employeeName,
            string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException(
                    "Employee email is required.",
                    nameof(email));
            }

            var message = new MimeMessage();

            var fromEmail = _configuration["Email:From"]
                ?? throw new InvalidOperationException(
                    "Email:From configuration is missing.");

            message.From.Add(
                MailboxAddress.Parse(fromEmail));

            message.To.Add(
                MailboxAddress.Parse(email));

            message.Subject =
                "Employee Created Successfully";

            message.Body = new TextPart("plain")
            {
                Text =
                    $"Hello {employeeName},\n\n" +
                    "Your employee record has been created successfully.\n\n" +
                    "Thank you."
            };

            using var smtp = new SmtpClient();

            var host = _configuration["Email:Host"]
                ?? throw new InvalidOperationException(
                    "Email:Host is missing.");

            var portValue = _configuration["Email:Port"]
                ?? throw new InvalidOperationException(
                    "Email:Port is missing.");

            var username = _configuration["Email:Username"]
                ?? throw new InvalidOperationException(
                    "Email:Username is missing");

            var password = _configuration["Email:Password"]
                ?? throw new InvalidOperationException(
                    "Email:Password is missing.");

            var port = int.Parse(portValue);

            // 1. Connect to Gmail SMTP
            await smtp.ConnectAsync(
                host,
                port,
                SecureSocketOptions.StartTls);

            // 2. Authenticate using Gmail App Password
            await smtp.AuthenticateAsync(
                username,
                password);

            // 3. Send email
            await smtp.SendAsync(message);

            // 4. Disconnect
            await smtp.DisconnectAsync(true);
        }
    }
}