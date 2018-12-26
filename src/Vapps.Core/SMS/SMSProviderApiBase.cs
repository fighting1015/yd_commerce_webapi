using Abp.Dependency;
using System.Threading.Tasks;
using Vapps.SMS.Cache;

namespace Vapps.SMS
{
    public abstract class SMSProviderApiBase : ISMSProviderApi, ITransientDependency
    {
        public SMSProviderInfo ProviderInfo { get; set; }

        public void Initialize(SMSProviderInfo providerInfo)
        {
            ProviderInfo = providerInfo;
        }

        /// <summary>
        /// 发送(内容/通知)短信
        /// </summary>
        /// <param name="targetNumbers">目标号码</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        public abstract Task<SendResult> SendAsync(string[] targetNumbers, string content);

        /// <summary>
        ///  发送验证码
        /// </summary>
        /// <param name="targetNumbers">目标号码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        public abstract Task<SendResult> SendCodeAsync(string targetNumbers, SendSMSTemplateResult sms);
    }
}
