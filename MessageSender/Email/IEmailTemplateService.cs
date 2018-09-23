namespace MessageSender.Email
{
    public interface IEmailTemplateService
    {
        string GetEmailBodyTemplate(string templateResourceName);
    }
}
