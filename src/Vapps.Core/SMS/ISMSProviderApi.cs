using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.SMS.Cache;

namespace Vapps.SMS
{
    public interface ISMSProviderApi
    {
        SMSProviderInfo ProviderInfo { get; }

        void Initialize(SMSProviderInfo providerInfo);

        /// <summary>
        /// 发送(内容/通知)短信
        /// </summary>
        /// <param name="targetNumbers">目标号码</param>
        /// <param name="content">内容</param>
        /// <returns></returns>
        Task<SendResult> SendAsync(string[] targetNumbers, string content);

        /// <summary>
        ///  发送验证码
        /// </summary>
        /// <param name="targetNumbers">目标号码</param>
        /// <param name="code">验证码</param>
        /// <returns></returns>
        Task<SendResult> SendCodeAsync(string targetNumber, SendSMSTemplateResult sms);
    }
}
