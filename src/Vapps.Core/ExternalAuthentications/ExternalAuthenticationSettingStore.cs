using Abp.Configuration;
using Abp.Dependency;
using Abp.Extensions;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Threading.Tasks;
using Vapps.Configuration;

namespace Vapps.ExternalAuthentications
{
    public class ExternalAuthenticationSettingStore : IExternalAuthenticationSettingStore, ITransientDependency
    {
        private readonly ISettingManager _settingManager;
        private readonly IConfigurationRoot _appConfiguration;

        public ExternalAuthenticationSettingStore(ISettingManager settingManager,
            IHostingEnvironment env)
        {
            _settingManager = settingManager;
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }

        public async Task<ExternalAuthenticationSetting> GetSettingsAsync(bool getProvider = false)
        {
            var providers = _appConfiguration["Authentication:Provider"];

            var externalAuthentication = new ExternalAuthenticationSetting()
            {
                UserActivationId = await _settingManager.GetSettingValueAsync<int>(AppSettings.ExternalAuthentication.UserActivation),
                RequiredUserName = await _settingManager.GetSettingValueAsync<bool>(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredUserName),
                RequiredEmail = await _settingManager.GetSettingValueAsync<bool>(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredEmail),
                RequiredTelephone = await _settingManager.GetSettingValueAsync<bool>(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredTelephone),
                UseTelephoneforUsername = await _settingManager.GetSettingValueAsync<bool>(AppSettings.ExternalAuthentication.UserActivationCondition.UseTelephoneforUsername),
            };

            if (!getProvider || providers.IsNullOrEmpty())
                return externalAuthentication;

            foreach (var provider in providers.Split(','))
            {
                var item = new ExternalAuthenticationProvider()
                {
                    ProviderName = provider,
                };

                var itemString = await _settingManager.GetSettingValueAsync(string.Format(AppSettings.ExternalAuthentication.ProviderName, provider));
                if (!itemString.IsNullOrEmpty())
                {
                    item = JsonConvert.DeserializeObject<ExternalAuthenticationProvider>(itemString);
                }

                externalAuthentication.ExternalAuthenticationProviders.Add(item);
            }

            return externalAuthentication;
        }
    }
}
