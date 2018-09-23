using System.Threading.Tasks;

namespace MessageSender.Email
{
    public interface IEmailService
    {
        Task SendMail(string toName, string toEmail, string subject, string emailBody, bool isHtmlBody = false);
    }
}
