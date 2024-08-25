
namespace EduProfileAPI.EmailService
{
    public interface IEmailService
    {
        Task SendEmailAsync(string from, string to, string subject, string htmlBody);
        //Task SendEmailAsync(string v, List<string> emails, string emailSubject, string emailBody);
    }
}

