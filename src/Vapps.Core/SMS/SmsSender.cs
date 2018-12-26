using System.Threading.Tasks;
using Abp.Dependency;
using Castle.Core.Logging;
using System;
using System.Linq;
using Vapps.SMS.Cache;

namespace Vapps.SMS
{
    /// <summary>
    /// 短信发送服务类
    /// </summary>
    public class SmsSender : ISmsSender, ITransientDependency
    {
        private readonly IIocResolver _iocResolver;
        private readonly ISMSConfiguration _smsConfiguration;

        public ILogger Logger { get; set; }

        public SmsSender(IIocResolver iocResolver,
            ISMSConfiguration smsConfiguratio)
        {
            _iocResolver = iocResolver;
            _smsConfiguration = smsConfiguratio;
            Logger = NullLogger.Instance;
        }


        public Task<SendResult> SendAsync(string[] targetNumbers, string content, string provider = "Alidayu")
        {
            using (var providerApi = CreateProviderApi(provider))
            {
                return providerApi.Object.SendAsync(targetNumbers, content);
            }
        }

        public Task<SendResult> SendAsync(string targetNumber, SendSMSTemplateResult sms, string provider = "Alidayu")
        {
            using (var providerApi = CreateProviderApi(provider))
            {
                return providerApi.Object.SendCodeAsync(targetNumber, sms);
            }
        }

        public IDisposableDependencyObjectWrapper<ISMSProviderApi> CreateProviderApi(string provider)
        {
            var providerInfo = _smsConfiguration.Providers.FirstOrDefault(p => p.Name == provider);
            if (providerInfo == null)
            {
                throw new Exception("Unknown external auth provider: " + provider);
            }

            var type = providerInfo.ProviderApiType;

            var providerApi = _iocResolver.ResolveAsDisposable<ISMSProviderApi>(providerInfo.ProviderApiType);
            providerApi.Object.Initialize(providerInfo);
            return providerApi;
        }
    }
}
