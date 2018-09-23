namespace MessageSender.Configurations
{
    public class BaseEmailConfig
    {
        public string MailServerName { get; set; }

        public int MailServerPort { get; set; }

        public string MailServerAuthUserName { get; set; }

        public string MailServerAuthPassword { get; set; }

        public string SenderName { get; set; }

        public string SenderEmail { get; set; }

        public string EmailAssemblyName { get; set; }
    }
}
