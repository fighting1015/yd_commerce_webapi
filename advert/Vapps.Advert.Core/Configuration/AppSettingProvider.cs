using Abp.Configuration;
using Abp.Extensions;
using Abp.Json;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Vapps.Advert.AdvertAccounts;
using Vapps.Configuration;
using Vapps.Enums;
using Vapps.ExternalAuthentications;

namespace Vapps.Advert.Configuration
{
    /// <summary>
    /// Defines settings for the application.
    /// See <see cref="AdvertSettings"/> for setting names.
    /// </summary>
    public class AdvertSettingProvider : SettingProvider
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AdvertSettingProvider(IHostingEnvironment env,
            IAppConfigurationAccessor configurationAccessor)
        {
            this._env = env;
            this._appConfiguration = configurationAccessor.Configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            var definitions = GetAdvertSettings()
                .ToList();

            return definitions;
        }
      
        private IEnumerable<SettingDefinition> GetAdvertSettings()
        {
            return new[]
            {
                new SettingDefinition(AdvertSettings.ToutiaoAdsAppId, GetFromAppSettings(AdvertSettings.ToutiaoAdsAppId,"1620174454145053"), scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(AdvertSettings.ToutiaoAdsAppSecret, GetFromAppSettings(AdvertSettings.ToutiaoAdsAppSecret,"41091c14794c7d24cd248a987f452fad1d8b4e15"), scopes: SettingScopes.Application, isVisibleToClients: false),
                new SettingDefinition(AdvertSettings.TenantAdsAppId, GetFromAppSettings(AdvertSettings.TenantAdsAppId,"1107493946"), scopes: SettingScopes.Application, isVisibleToClients: true),
                new SettingDefinition(AdvertSettings.TenantAdsAppSecret, GetFromAppSettings(AdvertSettings.TenantAdsAppSecret,"u1UbBRGXOTbEzdzv"), scopes: SettingScopes.Application, isVisibleToClients: false),
            };
        }

        private string GetFromAppSettings(string name, string defaultValue = null)
        {
            return _appConfiguration["App:" + name] ?? defaultValue;
        }

        private string GetFromSettings(string name, string defaultValue = null)
        {
            return _appConfiguration[name] ?? defaultValue;
        }
    }
}
