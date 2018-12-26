using Abp.Configuration;
using Abp.Dependency;
using Abp.Extensions;
using Castle.Core.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.ExternalAuthentications;

namespace Vapps.WeChat
{
    public class WeChatCommonHepler : ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly ISettingManager _settingManager;

        public WeChatCommonHepler(ISettingManager settingManager)
        {
            this._settingManager = settingManager;
            this._logger = NullLogger.Instance;
        }

        /// <summary>
        /// 获取微信登陆设置
        /// </summary>
        /// <returns></returns>
        public virtual ExternalAuthenticationProvider GetLoginProviderSetting(string ProviderName)
        {
            var loginProviderInfo = _settingManager
                .GetSettingValue(string.Format(AppSettings.ExternalAuthentication.ProviderName, ProviderName));

            ExternalAuthenticationProvider result = new ExternalAuthenticationProvider();
            if (!loginProviderInfo.IsNullOrEmpty())
            {
                result = JsonConvert.DeserializeObject<ExternalAuthenticationProvider>(loginProviderInfo);
            }

            return result;
        }

        /// <summary>
        /// 获取微信登陆设置
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ExternalAuthenticationProvider> GetLoginProviderSettingAsync(string ProviderName)
        {
            var loginProviderInfo = await _settingManager
                .GetSettingValueAsync(string.Format(AppSettings.ExternalAuthentication.ProviderName, ProviderName));

            ExternalAuthenticationProvider result = new ExternalAuthenticationProvider();
            if (!loginProviderInfo.IsNullOrEmpty())
            {
                result = JsonConvert.DeserializeObject<ExternalAuthenticationProvider>(loginProviderInfo);
            }

            return result;
        }
    }
}
