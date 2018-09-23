using System;
using System.IO;
using System.Linq;
using System.Reflection;
using MessageSender.Configurations;

namespace MessageSender.Email
{
    public class EmailTemplateService : IEmailTemplateService
    {
        private readonly BaseEmailConfig _config;

        public EmailTemplateService(BaseEmailConfig config)
        {
            _config = config;
        }

        public string GetEmailBodyTemplate(string templateResourceName)
        {
            if (string.IsNullOrEmpty(templateResourceName))
            {
                return string.Empty;
            }

            var assembly = Assembly.Load(_config.EmailAssemblyName);
            if (assembly == null)
            {
                return string.Empty;
            }

            if (!assembly.GetManifestResourceNames().Any(x => x.Equals(templateResourceName, StringComparison.OrdinalIgnoreCase)))
            {
                return string.Empty;
            }

            using (var resourceReader = new StreamReader(assembly.GetManifestResourceStream(templateResourceName)))
            {
                return resourceReader.ReadToEnd();
            }
        }
    }
}
