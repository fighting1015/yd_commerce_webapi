using System.Threading.Tasks;
using Abp.Net.Mail;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;
using System.Net.Mail;
using Abp.MailKit;
using Abp.Dependency;
using Abp.Localization;

namespace Vapps.Emailing
{
    public class CustomEmailSender : MailKitEmailSender
    {
        private readonly IMailKitSmtpBuilder _smtpBuilder;
        private readonly ILocalizationManager _localizationManager;

        public CustomEmailSender(
            IEmailSenderConfiguration smtpEmailSenderConfiguration,
            IMailKitSmtpBuilder smtpBuilder,
            ILocalizationManager localizationManager)
            : base(
                  smtpEmailSenderConfiguration, smtpBuilder)
        {
            _smtpBuilder = smtpBuilder;
            this._localizationManager = localizationManager;
        }

        public override async Task SendAsync(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            using (var client = BuildSmtpClient())
            {
                var message = BuildMimeMessage(from, L("Vapps"), to, subject, body, isBodyHtml);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
        }

        public override void Send(string from, string to, string subject, string body, bool isBodyHtml = true)
        {
            using (var client = BuildSmtpClient())
            {
                var message = BuildMimeMessage(from, L("Vapps"), to, subject, body, isBodyHtml);
                client.Send(message);
                client.Disconnect(true);
            }
        }

        private static MimeMessage BuildMimeMessage(string from, string fromName, string to, string subject, string body, bool isBodyHtml = true)
        {
            var bodyType = isBodyHtml ? "html" : "plain";
            var message = new MimeMessage
            {
                Subject = subject,
                Body = new TextPart(bodyType)
                {
                    Text = body
                }
            };

            message.From.Add(new MailboxAddress(fromName, from));
            message.To.Add(new MailboxAddress(to));

            return message;
        }

        private string L(string name)
        {
            return _localizationManager.GetString(VappsConsts.ServerSideLocalizationSourceName, name);
        }
    }
}
