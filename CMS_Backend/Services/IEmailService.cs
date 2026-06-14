namespace CMS_Backend.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(
            string toEmail,
            string subject,
            string htmlBody
        );
    }
}