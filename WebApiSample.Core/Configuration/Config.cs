using MessageSender.Configurations;

namespace WebApiSample.Core.Configuration
{
    public class Config
    {
        public ConnectionStringsConfig ConnectionStrings { get; set; }

        public BaseEmailConfig Email { get; set; }

        public IdentityConfig Identity { get; set; }
    }
}
