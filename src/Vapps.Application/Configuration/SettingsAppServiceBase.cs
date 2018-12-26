using System.Threading.Tasks;
using Abp.Net.Mail;
using Vapps.Configuration.Host.Dto;

namespace Vapps.Configuration
{
    public abstract class SettingsAppServiceBase : VappsAppServiceBase
    {
        private readonly IEmailSender _emailSender;

        protected SettingsAppServiceBase(
            IEmailSender emailSender)
        {
            _emailSender = emailSender;
        }

        #region Send Test Email

        /// <summary>
        /// 发送测试邮件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendTestEmail(SendTestEmailInput input)
        {
            await _emailSender.SendAsync(
                input.EmailAddress,
                L("TestEmail_Subject"),
                L("TestEmail_Body")
            );
        }

        #endregion
    }
}
