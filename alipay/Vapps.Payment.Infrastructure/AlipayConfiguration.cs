using Abp.Dependency;
using Abp.Extensions;
using Microsoft.Extensions.Configuration;
using Vapps.Configuration;

namespace Vapps.Alipay.Infrastructure
{
    public class AlipayConfiguration : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public string ServerAddress => _appConfiguration[ConfigKeys.ServerAddress].EnsureEndsWith('/');

        public bool IsEnable => _appConfiguration.GetValue<bool>(ConfigKeys.IsEnable);

        public string NotifyUrl => _appConfiguration[ConfigKeys.NotifyUrl].EnsureEndsWith('/');

        public string Uid => _appConfiguration[ConfigKeys.Pid];

        public string Gatewayurl => _appConfiguration[ConfigKeys.Gatewayurl];

        public string PrivateKey => _appConfiguration[ConfigKeys.PrivateKey];

        public string PublicKey => _appConfiguration[ConfigKeys.PublicKey];

        // 签名方式
        public string SignType => _appConfiguration[ConfigKeys.SignType];

        // 编码格式
        public string CharSet => _appConfiguration[ConfigKeys.CharSet];

        public AlipayConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }
}
