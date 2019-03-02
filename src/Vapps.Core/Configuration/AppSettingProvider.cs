using Abp.Configuration;
using Abp.Extensions;
using Abp.Json;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using Vapps.Enums;
using Vapps.ExternalAuthentications;

namespace Vapps.Configuration
{
    /// <summary>
    /// Defines settings for the application.
    /// See <see cref="AppSettings"/> for setting names.
    /// </summary>
    public class AppSettingProvider : SettingProvider
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public AppSettingProvider(IHostingEnvironment env,
            IAppConfigurationAccessor configurationAccessor)
        {
            this._env = env;
            this._appConfiguration = configurationAccessor.Configuration;
        }

        public override IEnumerable<SettingDefinition> GetSettingDefinitions(SettingDefinitionProviderContext context)
        {
            //Disable TwoFactorLogin by default (can be enabled by UI)
            context.Manager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled).DefaultValue = false.ToString().ToLowerInvariant();

            var definitions = GetHostSettings().Union(GetTenantSettings())
                .Union(GetExternalAuthenticationSettings())
                .Union(GetSmsManagementSettings())
                .Union(GetVerificationCodeManagementSettings()).ToList();

            var providers = _appConfiguration["Authentication:Provider"];
            if (!providers.IsNullOrEmpty())
            {
                foreach (var provider in providers.Split(','))
                {
                    var defaultExternalAuthentication = new ExternalAuthenticationProvider
                    {
                        ProviderName = provider,
                        IsEnabled = false,
                        ShowOnLoginPage = true,
                    };
                    definitions.Add(new SettingDefinition(string.Format(AppSettings.ExternalAuthentication.ProviderName, provider), defaultExternalAuthentication.ToJsonString(), scopes: SettingScopes.Application, isVisibleToClients: false));
                }
            }

            return definitions;
        }

        private IEnumerable<SettingDefinition> GetHostSettings()
        {
            return new[]{
              //Host settings
              new SettingDefinition(AppSettings.TenantManagement.AllowSelfRegistration, GetFromAppSettings(AppSettings.TenantManagement.AllowSelfRegistration, "true"), isVisibleToClients: true),
              new SettingDefinition(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault, GetFromAppSettings(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault, "false")),
              new SettingDefinition(AppSettings.TenantManagement.UseCaptchaOnRegistration, GetFromAppSettings(AppSettings.TenantManagement.UseCaptchaOnRegistration, "true"), isVisibleToClients: true),
              new SettingDefinition(AppSettings.TenantManagement.DefaultEdition, GetFromAppSettings(AppSettings.TenantManagement.DefaultEdition, "")),
              new SettingDefinition(AppSettings.TenantManagement.SubscriptionExpireNotifyDayCount, GetFromAppSettings(AppSettings.TenantManagement.SubscriptionExpireNotifyDayCount, "7"), isVisibleToClients: true),
              new SettingDefinition(AppSettings.Recaptcha.SiteKey, GetFromSettings("Recaptcha:SiteKey"), isVisibleToClients: true),
    
            };
        }

        private IEnumerable<SettingDefinition> GetTenantSettings()
        {
            return new[]
            {
               new SettingDefinition(AppSettings.UserManagement.AllowSelfRegistration, GetFromAppSettings(AppSettings.UserManagement.UseCaptchaOnRegistration,"true"), scopes: SettingScopes.Tenant, isVisibleToClients: true),
               new SettingDefinition(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault, GetFromAppSettings(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault, "false"), scopes: SettingScopes.Tenant),
               new SettingDefinition(AppSettings.UserManagement.UseCaptchaOnRegistration, GetFromAppSettings(AppSettings.UserManagement.UseCaptchaOnRegistration, "true"), scopes: SettingScopes.Tenant, isVisibleToClients: true),
               new SettingDefinition(AppSettings.TenantManagement.BillingLegalName, GetFromAppSettings(AppSettings.TenantManagement.BillingLegalName, ""), scopes: SettingScopes.Tenant),
               new SettingDefinition(AppSettings.TenantManagement.BillingAddress, GetFromAppSettings(AppSettings.TenantManagement.BillingAddress, ""), scopes: SettingScopes.Tenant),
               new SettingDefinition(AppSettings.TenantManagement.BillingTaxVatNo, GetFromAppSettings(AppSettings.TenantManagement.BillingTaxVatNo, ""), scopes: SettingScopes.Tenant)
            };
        }

        private IEnumerable<SettingDefinition> GetExternalAuthenticationSettings()
        {
            return new[]
            {
               //External authentication settings
               new SettingDefinition(AppSettings.ExternalAuthentication.UserActivation, GetFromAppSettings(AppSettings.ExternalAuthentication.UserActivation,
                 ((int)UserActivationOption.DonotActive).ToString()), scopes: SettingScopes.Application, isVisibleToClients: true),
               new SettingDefinition(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredUserName, GetFromAppSettings(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredUserName,"false"),scopes: SettingScopes.Application, isVisibleToClients: true),
               new SettingDefinition(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredEmail, GetFromAppSettings(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredEmail,"false"),scopes: SettingScopes.Application, isVisibleToClients: true),
               new SettingDefinition(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredTelephone, GetFromAppSettings(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredTelephone,"false"),scopes: SettingScopes.Application, isVisibleToClients: true),
               new SettingDefinition(AppSettings.ExternalAuthentication.UserActivationCondition.UseTelephoneforUsername, GetFromAppSettings(AppSettings.ExternalAuthentication.UserActivationCondition.UseTelephoneforUsername,"false"),scopes: SettingScopes.Application, isVisibleToClients: true),
            };
        }

        private IEnumerable<SettingDefinition> GetSmsManagementSettings()
        {
            return new[]
            {
              new SettingDefinition(AppSettings.SMSManagement.SmsVerificationEnabled, GetFromAppSettings(AppSettings.SMSManagement.SmsVerificationEnabled, "false"), scopes: SettingScopes.Application, isVisibleToClients: true),
              new SettingDefinition(AppSettings.SMSManagement.UseCaptchaToVerification, GetFromAppSettings(AppSettings.SMSManagement.UseCaptchaToVerification, "false"), scopes: SettingScopes.Application, isVisibleToClients: true),
              new SettingDefinition(AppSettings.SMSManagement.RegisterVerificationTempId, GetFromAppSettings(AppSettings.SMSManagement.RegisterVerificationTempId, "0"), scopes: SettingScopes.Application, isVisibleToClients: false),
              new SettingDefinition(AppSettings.SMSManagement.ChangePasswordVerificationTempId, GetFromAppSettings(AppSettings.SMSManagement.ChangePasswordVerificationTempId, "0"), scopes: SettingScopes.Application, isVisibleToClients: false),
              new SettingDefinition(AppSettings.SMSManagement.BindingPhoneVerificationTempId, GetFromAppSettings(AppSettings.SMSManagement.BindingPhoneVerificationTempId, "0"), scopes: SettingScopes.Application, isVisibleToClients: false),
              new SettingDefinition(AppSettings.SMSManagement.UnBindingPhoneVerificationTempId, GetFromAppSettings(AppSettings.SMSManagement.UnBindingPhoneVerificationTempId, "0"), scopes: SettingScopes.Application, isVisibleToClients: false),
              new SettingDefinition(AppSettings.SMSManagement.LoginVerificationTempId, GetFromAppSettings(AppSettings.SMSManagement.LoginVerificationTempId, "0"), scopes: SettingScopes.Application, isVisibleToClients: false),
              new SettingDefinition(AppSettings.SMSManagement.PhoneVerificationTempId, GetFromAppSettings(AppSettings.SMSManagement.PhoneVerificationTempId, "0"), scopes: SettingScopes.Application, isVisibleToClients: false),
            };
        }

        private IEnumerable<SettingDefinition> GetVerificationCodeManagementSettings()
        {
            return new[]
            {
                //Verification code management
                new SettingDefinition(AppSettings.UserManagement.VerificationCodeManagement.IsEnabled, GetFromAppSettings(AppSettings.UserManagement.VerificationCodeManagement.IsEnabled, "false")),
                new SettingDefinition(AppSettings.UserManagement.VerificationCodeManagement.AvailableSecond, GetFromAppSettings(AppSettings.UserManagement.VerificationCodeManagement.AvailableSecond, "600")),
                new SettingDefinition(AppSettings.UserManagement.VerificationCodeManagement.MinimumSendInterval, GetFromAppSettings(AppSettings.UserManagement.VerificationCodeManagement.MinimumSendInterval, "60"))
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
