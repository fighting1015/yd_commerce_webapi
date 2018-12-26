using Abp.Dependency;
using Abp.Extensions;
using Microsoft.Extensions.Configuration;
using Vapps.Configuration;

namespace Vapps.WeChat
{
    public class WeChatConfiguration : ITransientDependency
    {
        private readonly IConfigurationRoot _appConfiguration;

        public string ServerAddress => _appConfiguration[ConfigKeys.ServerAddress].EnsureEndsWith('/');

        public bool IsEnable => _appConfiguration.GetValue<bool>(ConfigKeys.IsEnable);

        public string NotifyUrl => _appConfiguration[ConfigKeys.NotifyUrl].EnsureEndsWith('/');

        public string AppId => _appConfiguration[ConfigKeys.AppId];

        public string TenPayKey => _appConfiguration[ConfigKeys.TenPayKey];

        public string MerchantId => _appConfiguration[ConfigKeys.MerchantId];

        public WeChatConfiguration(IAppConfigurationAccessor configurationAccessor)
        {
            _appConfiguration = configurationAccessor.Configuration;
        }
    }
}
