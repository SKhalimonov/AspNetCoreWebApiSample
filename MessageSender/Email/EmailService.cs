using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System.Net;
using System.Threading.Tasks;
using MessageSender.Configurations;

namespace MessageSender.Email
{
    public class EmailService : IEmailService
    {
        private readonly BaseEmailConfig _config;

        public EmailService(BaseEmailConfig emailConfig)
        {
            _config = emailConfig;
        }

        public async Task SendMail(string toName, string toEmail, string subject, string emailBody, bool isHtmlBody = false)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_config.SenderName, _config.SenderEmail));
            emailMessage.To.Add(new MailboxAddress(toName, toEmail));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(isHtmlBody ? TextFormat.Html : TextFormat.Plain) { Text = emailBody };

            using (var client = new SmtpClient())
            {
                var credentials = new NetworkCredential
                {
                    UserName = _config.MailServerAuthUserName,
                    Password = _config.MailServerAuthPassword
                };

                await client.ConnectAsync(_config.MailServerName, _config.MailServerPort, false).ConfigureAwait(false);

                if (client.Capabilities.HasFlag(SmtpCapabilities.Authentication))
                {
                    await client.AuthenticateAsync(credentials).ConfigureAwait(false);
                }

                await client.SendAsync(emailMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
        }
    }
}
