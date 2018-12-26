using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Json;
using Abp.Net.Mail;
using Abp.Runtime.Security;
using Abp.Timing;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Globalization;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Configuration.Host.Dto;
using Vapps.Editions;
using Vapps.Security;
using Vapps.Timing;

namespace Vapps.Configuration.Host
{
    /// <summary>
    /// 宿主设置
    /// </summary>
    [AbpAuthorize(AdminPermissions.Configuration.HostSettings)]
    public class HostSettingsAppService : SettingsAppServiceBase, IHostSettingsAppService
    {
        private readonly EditionManager _editionManager;

        private readonly ITimeZoneService _timeZoneService;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ISettingDefinitionManager _settingDefinitionManager;

        public HostSettingsAppService(
            EditionManager editionManager,
            IEmailSender emailSender,
            ITimeZoneService timeZoneService,
            ISettingDefinitionManager settingDefinitionManager,
            IHostingEnvironment env) : base(emailSender)
        {
            _editionManager = editionManager;
            _timeZoneService = timeZoneService;
            _settingDefinitionManager = settingDefinitionManager;
            _appConfiguration = AppConfigurations.Get(env.ContentRootPath, env.EnvironmentName, env.IsDevelopment());
        }

        #region Methods

        /// <summary>
        /// 获取宿主所有设置
        /// </summary>
        /// <returns></returns>
        public async Task<HostSettingsEditDto> GetAllSettings()
        {
            return new HostSettingsEditDto
            {
                General = await GetGeneralSettingsAsync(),
                TenantManagement = await GetTenantManagementSettingsAsync(),
                UserManagement = await GetUserManagementAsync(),
                Email = await GetEmailSettingsAsync(),
                Security = await GetSecuritySettingsAsync(),
                ExternalAuthentication = await GetExternalAuthenticationsAsync(),
                SMSSettings = await GetSMSSettingsAsync(),
                Billing = await GetBillingSettingsAsync()
            };
        }

        /// <summary>
        /// 更新宿主所有设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateAllSettings(HostSettingsEditDto input)
        {
            await UpdateGeneralSettingsAsync(input.General);
            await UpdateTenantManagementAsync(input.TenantManagement);
            await UpdateUserManagementSettingsAsync(input.UserManagement);
            await UpdateSecuritySettingsAsync(input.Security);
            await UpdateEmailSettingsAsync(input.Email);
            await UpdateExternalAuthenticationsAsync(input.ExternalAuthentication);
            //await UpdateSMSSettingsAsync(input.SMSSettings);
            //await UpdateBillingSettingsAsync(input.Billing);
        }

        /// <summary>
        /// 更新账单信息设置
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task UpdateBillingSettingsAsync(HostBillingSettingsEditDto input)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingLegalName, input.LegalName);
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.HostManagement.BillingAddress, input.Address);

        }

        /// <summary>
        /// 更新第三方登陆配置(单条记录)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateExternalAuthentication(ExternalAuthenticationProviderEditDto input)
        {
            await UpdateExternalAuthenticationAsync(input);
        }

        #endregion

        #region Utilities 

        #region Get Settings

        /// <summary>
        /// 获取基本设置
        /// </summary>
        /// <returns></returns>
        private async Task<GeneralSettingsEditDto> GetGeneralSettingsAsync()
        {
            var timezone = await SettingManager.GetSettingValueForApplicationAsync(TimingSettingNames.TimeZone);
            var settings = new GeneralSettingsEditDto
            {
                Timezone = timezone,
                TimezoneForComparison = timezone
            };

            var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
            if (settings.Timezone == defaultTimeZoneId)
            {
                settings.Timezone = string.Empty;
            }

            return settings;
        }

        /// <summary>
        /// 获取租户设置
        /// </summary>
        /// <returns></returns>
        private async Task<TenantManagementSettingsEditDto> GetTenantManagementSettingsAsync()
        {
            var settings = new TenantManagementSettingsEditDto
            {
                AllowSelfRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.AllowSelfRegistration),
                IsNewRegisteredTenantActiveByDefault = await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault),
                UseCaptchaOnRegistration = await SettingManager.GetSettingValueAsync<bool>(AppSettings.TenantManagement.UseCaptchaOnRegistration)
            };

            var defaultEditionId = await SettingManager.GetSettingValueAsync(AppSettings.TenantManagement.DefaultEdition);
            if (!string.IsNullOrEmpty(defaultEditionId) && (await _editionManager.FindByIdAsync(Convert.ToInt32(defaultEditionId)) != null))
            {
                settings.DefaultEditionId = Convert.ToInt32(defaultEditionId);
            }

            return settings;
        }

        /// <summary>
        /// 获取用户设置
        /// </summary>
        /// <returns></returns>
        private async Task<HostUserManagementSettingsEditDto> GetUserManagementAsync()
        {
            return new HostUserManagementSettingsEditDto
            {
                IsEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin)
            };
        }

        /// <summary>
        /// 获取邮件设置
        /// </summary>
        /// <returns></returns>
        private async Task<EmailSettingsEditDto> GetEmailSettingsAsync()
        {
            var smtpPassword = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Password);

            return new EmailSettingsEditDto
            {
                DefaultFromAddress = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromAddress),
                DefaultFromDisplayName = await SettingManager.GetSettingValueAsync(EmailSettingNames.DefaultFromDisplayName),
                SmtpHost = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Host),
                SmtpPort = await SettingManager.GetSettingValueAsync<int>(EmailSettingNames.Smtp.Port),
                SmtpUserName = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.UserName),
                SmtpPassword = SimpleStringCipher.Instance.Decrypt(smtpPassword),
                SmtpDomain = await SettingManager.GetSettingValueAsync(EmailSettingNames.Smtp.Domain),
                SmtpEnableSsl = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.EnableSsl),
                SmtpUseDefaultCredentials = await SettingManager.GetSettingValueAsync<bool>(EmailSettingNames.Smtp.UseDefaultCredentials)
            };
        }

        /// <summary>
        /// 获取安全设置
        /// </summary>
        /// <returns></returns>
        private async Task<SecuritySettingsEditDto> GetSecuritySettingsAsync()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                RequireLowercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                RequiredLength = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
            };

            var defaultPasswordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit).DefaultValue),
                RequireLowercase = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase).DefaultValue),
                RequireNonAlphanumeric = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric).DefaultValue),
                RequireUppercase = Convert.ToBoolean(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase).DefaultValue),
                RequiredLength = Convert.ToInt32(_settingDefinitionManager.GetSettingDefinition(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength).DefaultValue)
            };

            return new SecuritySettingsEditDto
            {
                UseDefaultPasswordComplexitySettings = passwordComplexitySetting.Equals(defaultPasswordComplexitySetting),
                PasswordComplexity = passwordComplexitySetting,
                DefaultPasswordComplexity = defaultPasswordComplexitySetting,
                UserLockOut = await GetUserLockOutSettingsAsync(),
                TwoFactorLogin = await GetTwoFactorLoginSettingsAsync(),
                VerificationCode = await GetVerificationCodeSettingsAsync()
            };
        }

        /// <summary>
        /// 获取账单设置
        /// </summary>
        /// <returns></returns>
        private async Task<HostBillingSettingsEditDto> GetBillingSettingsAsync()
        {
            return new HostBillingSettingsEditDto
            {
                LegalName = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingLegalName),
                Address = await SettingManager.GetSettingValueAsync(AppSettings.HostManagement.BillingAddress)
            };
        }

        /// <summary>
        /// 获取用户锁定设置
        /// </summary>
        /// <returns></returns>
        private async Task<UserLockOutSettingsEditDto> GetUserLockOutSettingsAsync()
        {
            return new UserLockOutSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled),
                MaxFailedAccessAttemptsBeforeLockout = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout),
                DefaultAccountLockoutSeconds = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds)
            };
        }

        /// <summary>
        /// 获取两步认证登陆设置
        /// </summary>
        /// <returns></returns>
        private async Task<TwoFactorLoginSettingsEditDto> GetTwoFactorLoginSettingsAsync()
        {
            return new TwoFactorLoginSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled),
                IsEmailProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled),
                IsSmsProviderEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled),
                IsRememberBrowserEnabled = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled),
            };
        }

        /// <summary>
        /// 获取验证码设置
        /// </summary>
        /// <returns></returns>
        public async Task<VerificationCodeSettingsEditDto> GetVerificationCodeSettingsAsync()
        {
            return new VerificationCodeSettingsEditDto
            {
                IsEnabled = await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.VerificationCodeManagement.IsEnabled),
                AvailableSecond = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.VerificationCodeManagement.AvailableSecond),
                MinimumSendInterval = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.VerificationCodeManagement.MinimumSendInterval),
            };
        }

        /// <summary>
        /// 获取外部认证设置
        /// </summary>
        /// <returns></returns>
        private async Task<ExternalAuthenticationEditDto> GetExternalAuthenticationsAsync()
        {
            var providers = _appConfiguration["Authentication:Provider"];

            var externalAuthentication = new ExternalAuthenticationEditDto()
            {
                UserActivationId = await SettingManager.GetSettingValueAsync<int>(AppSettings.ExternalAuthentication.UserActivation),
                RequiredUserName = await SettingManager.GetSettingValueAsync<bool>(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredUserName),
                RequiredEmail = await SettingManager.GetSettingValueAsync<bool>(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredEmail),
                RequiredTelephone = await SettingManager.GetSettingValueAsync<bool>(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredTelephone),
                UseTelephoneforUsername = await SettingManager.GetSettingValueAsync<bool>(AppSettings.ExternalAuthentication.UserActivationCondition.UseTelephoneforUsername),
            };

            if (!providers.IsNullOrEmpty())
            {
                foreach (var provider in providers.Split(','))
                {
                    var item = new ExternalAuthenticationProviderEditDto()
                    {
                        ProviderName = provider,
                    };

                    var itemString = await SettingManager.GetSettingValueAsync(string.Format(AppSettings.ExternalAuthentication.ProviderName, provider));
                    if (!itemString.IsNullOrEmpty())
                    {
                        item = JsonConvert.DeserializeObject<ExternalAuthenticationProviderEditDto>(itemString);
                    }

                    externalAuthentication.ExternalAuthenticationProviders.Add(item);
                }
            }

            return externalAuthentication;
        }

        /// <summary>
        /// 获取短信设置
        /// </summary>
        /// <returns></returns>
        private async Task<SMSSettingsEditDto> GetSMSSettingsAsync()
        {
            return new SMSSettingsEditDto
            {
                UseCaptchaToVerification = await SettingManager.GetSettingValueAsync<bool>(AppSettings.SMSManagement.UseCaptchaToVerification),
                RegisterVerificationTempId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.RegisterVerificationTempId),
                LoginVerificationTempId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.LoginVerificationTempId),
                PhoneVerificationTempId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.PhoneVerificationTempId),
                ChangePasswordVerificationTempId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.ChangePasswordVerificationTempId),
                UnBindingPhoneVerificationTempId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.UnBindingPhoneVerificationTempId),
                BindingPhoneVerificationTempId = await SettingManager.GetSettingValueAsync<int>(AppSettings.SMSManagement.BindingPhoneVerificationTempId),
            };
        }

        #endregion

        #region Update Settings

        /// <summary>
        /// 更新基础设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateGeneralSettingsAsync(GeneralSettingsEditDto settings)
        {
            if (Clock.SupportsMultipleTimezone)
            {
                if (settings.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.Application, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForApplicationAsync(TimingSettingNames.TimeZone, settings.Timezone);
                }
            }
        }

        /// <summary>
        /// 更新租户管理设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateTenantManagementAsync(TenantManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.AllowSelfRegistration,
                settings.AllowSelfRegistration.ToLowerString()
            );
            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault,
                settings.IsNewRegisteredTenantActiveByDefault.ToLowerString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.UseCaptchaOnRegistration,
                settings.UseCaptchaOnRegistration.ToLowerString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AppSettings.TenantManagement.DefaultEdition,
                settings.DefaultEditionId?.ToString() ?? ""
            );
        }

        /// <summary>
        /// 更新用户管理设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateUserManagementSettingsAsync(HostUserManagementSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin,
                settings.IsEmailConfirmationRequiredForLogin.ToLowerString()
            );
        }

        /// <summary>
        /// 更新安全设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateSecuritySettingsAsync(SecuritySettingsEditDto settings)
        {
            if (settings.UseDefaultPasswordComplexitySettings)
            {
                await UpdatePasswordComplexitySettingsAsync(settings.DefaultPasswordComplexity);
            }
            else
            {
                await UpdatePasswordComplexitySettingsAsync(settings.PasswordComplexity);
            }

            await UpdateUserLockOutSettingsAsync(settings.UserLockOut);
            await UpdateTwoFactorLoginSettingsAsync(settings.TwoFactorLogin);
            await UpdateVerificationCodeSettingsAsync(settings.VerificationCode);
        }

        private async Task UpdatePasswordComplexitySettingsAsync(PasswordComplexitySetting settings)
        {

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit,
                settings.RequireDigit.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase,
                settings.RequireLowercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric,
                settings.RequireNonAlphanumeric.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase,
                settings.RequireUppercase.ToString()
            );

            await SettingManager.ChangeSettingForApplicationAsync(
                AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength,
                settings.RequiredLength.ToString()
            );
        }

        /// <summary>
        /// 更新用户锁定设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateUserLockOutSettingsAsync(UserLockOutSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.IsEnabled, settings.IsEnabled.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.DefaultAccountLockoutSeconds, settings.DefaultAccountLockoutSeconds.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.UserLockOut.MaxFailedAccessAttemptsBeforeLockout, settings.MaxFailedAccessAttemptsBeforeLockout.ToString());
        }

        /// <summary>
        /// 更新两步登陆设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateTwoFactorLoginSettingsAsync(TwoFactorLoginSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled, settings.IsEnabled.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEmailProviderEnabled, settings.IsEmailProviderEnabled.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsSmsProviderEnabled, settings.IsSmsProviderEnabled.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled, settings.IsRememberBrowserEnabled.ToLowerString());
        }

        /// <summary>
        /// 更新验证码设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateVerificationCodeSettingsAsync(VerificationCodeSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.UserManagement.VerificationCodeManagement.IsEnabled, settings.IsEnabled.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.UserManagement.VerificationCodeManagement.AvailableSecond, settings.AvailableSecond.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.UserManagement.VerificationCodeManagement.MinimumSendInterval, settings.MinimumSendInterval.ToString());
        }

        /// <summary>
        /// 更新邮件设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateEmailSettingsAsync(EmailSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromAddress, settings.DefaultFromAddress);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.DefaultFromDisplayName, settings.DefaultFromDisplayName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Host, settings.SmtpHost);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Port, settings.SmtpPort.ToString(CultureInfo.InvariantCulture));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UserName, settings.SmtpUserName);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Password, SimpleStringCipher.Instance.Encrypt(settings.SmtpPassword));
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.Domain, settings.SmtpDomain);
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.EnableSsl, settings.SmtpEnableSsl.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(EmailSettingNames.Smtp.UseDefaultCredentials, settings.SmtpUseDefaultCredentials.ToLowerString());
        }

        /// <summary>
        /// 更新外部登陆
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateExternalAuthenticationsAsync(ExternalAuthenticationEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.ExternalAuthentication.UserActivation,
               settings.UserActivationId.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredEmail,
                settings.RequiredEmail.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredUserName,
                settings.RequiredUserName.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.ExternalAuthentication.UserActivationCondition.RequiredTelephone,
                settings.RequiredTelephone.ToLowerString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.ExternalAuthentication.UserActivationCondition.UseTelephoneforUsername,
              settings.UseTelephoneforUsername.ToLowerString());

            // 更新外部登陆供应商
            foreach (var item in settings.ExternalAuthenticationProviders)
            {
                await UpdateExternalAuthenticationAsync(item);
            }
        }

        /// <summary>
        /// 更新外部登陆
        /// </summary>
        /// <param name="setting"></param>
        /// <returns></returns>
        private async Task UpdateExternalAuthenticationAsync(ExternalAuthenticationProviderEditDto setting)
        {
            var providers = _appConfiguration["Authentication:Provider"];

            if (!providers.Contains(setting.ProviderName))
                return;

            //保存单条设置
            await SettingManager.ChangeSettingForApplicationAsync(string.Format(AppSettings.ExternalAuthentication.ProviderName, setting.ProviderName),
                setting.ToJsonString());
        }

        /// <summary>
        /// 更新短信设置
        /// </summary>
        /// <param name="settings"></param>
        /// <returns></returns>
        private async Task UpdateSMSSettingsAsync(SMSSettingsEditDto settings)
        {
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.SMSManagement.UseCaptchaToVerification, settings.UseCaptchaToVerification.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.SMSManagement.RegisterVerificationTempId, settings.RegisterVerificationTempId.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.SMSManagement.ChangePasswordVerificationTempId, settings.ChangePasswordVerificationTempId.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.SMSManagement.BindingPhoneVerificationTempId, settings.BindingPhoneVerificationTempId.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.SMSManagement.UnBindingPhoneVerificationTempId, settings.UnBindingPhoneVerificationTempId.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.SMSManagement.LoginVerificationTempId,
              settings.LoginVerificationTempId.ToString());
            await SettingManager.ChangeSettingForApplicationAsync(AppSettings.SMSManagement.PhoneVerificationTempId,
              settings.PhoneVerificationTempId.ToString());
        }

        #endregion

        #endregion
    }
}