
using Microsoft.Extensions.Options;
using MimeKit;
using System.Net.Mail;
using MailKit.Net.Smtp;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace EduProfileAPI.EmailService
{
    public class MailKitEmailService : IEmailService
    {
        private readonly EmailConfiguration _emailConfig;

        public MailKitEmailService(IOptions<EmailConfiguration> emailConfig)
        {
            _emailConfig = emailConfig.Value;  // This sets the configuration once and uses it throughout the service.
        }

        public async Task SendEmailAsync(string from, string to, string subject, string htmlBody)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("no-reply", from));
            emailMessage.To.Add(new MailboxAddress("Recipient Name", to));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = htmlBody };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailConfig.Host, _emailConfig.Port, _emailConfig.UseSSL);
                await client.AuthenticateAsync(_emailConfig.Username, _emailConfig.Password);
                await client.SendAsync(emailMessage);
                await client.DisconnectAsync(true);
            }
        }
    }
}
