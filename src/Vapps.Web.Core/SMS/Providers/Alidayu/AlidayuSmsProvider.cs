using Abp.Dependency;
using Aliyun.Acs.Core;
using Aliyun.Acs.Core.Exceptions;
using Aliyun.Acs.Core.Profile;
using Aliyun.Acs.Dysmsapi.Model.V20170525;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using Vapps.SMS;

namespace Vapps.Web.SMS.Providers.Alidayu
{
    public class AlidayuProvider : SMSProviderApiBase, ITransientDependency
    {
        public const string Name = "Alidayu";
        private readonly string _product = "Dysmsapi";//短信API产品名称
        private readonly string _smsFreeSignName = "小预约";
        private readonly string _domain = "dysmsapi.aliyuncs.com";//短信API产品域名

        private readonly ILogger _logger;

        public AlidayuProvider(ILogger logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// 发送(内容/通知)短信
        /// </summary>
        /// <param name="targetNumbers"></param>
        /// <param name="content"></param>
        /// <returns></returns>
        public override async Task<SendResult> SendAsync(string[] targetNumbers, string content)
        {
            return await Task.FromResult(new SendResult());
        }

        /// <summary>
        /// 发送注册验证码
        /// </summary>
        /// <param name="targetNumber"></param>
        /// <param name="sms"></param>
        /// <returns></returns>
        public override async Task<SendResult> SendCodeAsync(string targetNumber, SendSMSTemplateResult sms)
        {
            var param = new Dictionary<string, string>();
            sms.Items.Each(x =>
            {
                param.Add(x.DataItemName, x.DataItemValue);
            });

            return await SendSms(ProviderInfo.AppId, ProviderInfo.AppSecret, sms.TemplateCode, param, new string[] { targetNumber });
        }

        /// <summary>
        /// Sends SMS
        /// </summary>
        /// <param name="secret"></param>
        /// <param name="appKey"></param>
        /// <param name="para"></param>
        /// <param name="targetNumbers"></param>
        /// <param name="templateCode"></param>
        /// <returns>Result</returns>
        private async Task<SendResult> SendSms(string appKey, string secret, string templateCode, Dictionary<string, string> para, string[] targetNumbers)
        {
            IClientProfile profile = DefaultProfile.GetProfile("cn-hangzhou", appKey, secret);

            DefaultProfile.AddEndpoint("cn-hangzhou", "cn-hangzhou", _product, _domain);
            IAcsClient acsClient = new DefaultAcsClient(profile);
            SendSmsRequest request = new SendSmsRequest();
            try
            {
                //必填:待发送手机号。支持以逗号分隔的形式进行批量调用，批量上限为20个手机号码,批量调用相对于单条调用及时性稍有延迟,验证码类型的短信推荐使用单条调用的方式
                request.PhoneNumbers = string.Join(",", targetNumbers);
                //必填:短信签名-可在短信控制台中找到
                request.SignName = _smsFreeSignName;
                //必填:短信模板-可在短信控制台中找到
                request.TemplateCode = templateCode;
                //可选:模板中的变量替换JSON串,如模板内容为"亲爱的${name},您的验证码为${code}"时,此处的值为
                request.TemplateParam = para.GetString();
                //可选:outId为提供给业务方扩展字段,最终在短信回执消息中将此值带回给调用者
                SendSmsResponse sendSmsResponse = acsClient.GetAcsResponse(request);

                SendResult result = new SendResult()
                {
                    Success = sendSmsResponse.HttpResponse.isSuccess(),
                    ErrorMessage = sendSmsResponse.Message,
                };

                return await Task.FromResult(result);
            }
            catch (ServerException ex)
            {
                _logger.Error(ex.Message, ex);
                return new SendResult()
                {
                    ErrorMessage = ex.Message,
                    Success = false
                };
            }
            catch (ClientException ex)
            {
                _logger.Error(ex.Message, ex);
                return new SendResult()
                {
                    ErrorMessage = ex.Message,
                    Success = false
                };
            }
        }
    }
}
