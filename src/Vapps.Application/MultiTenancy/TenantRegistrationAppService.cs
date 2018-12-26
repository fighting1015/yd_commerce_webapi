using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.UI;
using Abp.Zero.Configuration;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Roles;
using Vapps.Configuration;
using Vapps.Debugging;
using Vapps.Editions;
using Vapps.Identity;
using Vapps.MultiTenancy.Dto;
using Vapps.Notifications;
using Vapps.Security.Recaptcha;
using Vapps.Url;

namespace Vapps.MultiTenancy
{
    public class TenantRegistrationAppService : VappsAppServiceBase, ITenantRegistrationAppService
    {
        private readonly IAppUrlService _appUrlService;
        private readonly IMultiTenancyConfig _multiTenancyConfig;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IAppNotifier _appNotifier;
        private readonly IVerificationCodeManager _verificationCodeManager;

        private readonly EditionManager _editionManager;
        private readonly RoleManager _roleManager;

        public TenantRegistrationAppService(
            IAppUrlService appUrlService,
            IMultiTenancyConfig multiTenancyConfig,
            ICaptchaValidator recaptchaValidator,
            IAppNotifier appNotifier,
            IVerificationCodeManager verificationCodeManager,
            EditionManager editionManager,
            RoleManager roleManager)
        {
            this._multiTenancyConfig = multiTenancyConfig;
            this._captchaValidator = recaptchaValidator;
            this._editionManager = editionManager;
            this._appNotifier = appNotifier;
            this._appUrlService = appUrlService;
            this._verificationCodeManager = verificationCodeManager;
            this._roleManager = roleManager;
        }

        /// <summary>
        /// 租户注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                CheckTenantRegistrationIsEnabled();

                //if (UseCaptchaOnRegistration())//使用手机验证码作为验证
                //{
                //    await _captchaValidator.ValidateAsync(input.CaptchaResponse);
                //}

                switch (input.Type)
                {
                    case RegisterType.Telephone:
                        {
                            if (input.PhoneNumber.IsNullOrWhiteSpace())
                                throw new UserFriendlyException(L("Identity.RequiredPhoneNumber"));

                            await _verificationCodeManager.CheckRegistrationVerificationCode(input.PhoneNumber, input.RegisterCode);
                            break;
                        }
                    case RegisterType.Email:
                        {
                            if (input.EmailAddress.IsNullOrWhiteSpace())
                                throw new UserFriendlyException(L("Identity.RequiredEmail"));

                            await _verificationCodeManager.CheckRegistrationVerificationCode(input.EmailAddress, input.RegisterCode);
                            break;
                        }
                    default:
                        break;
                }

                //Getting host-specific settings
                var isNewRegisteredTenantActiveByDefault = await SettingManager.GetSettingValueForApplicationAsync<bool>(AppSettings.TenantManagement.IsNewRegisteredTenantActiveByDefault);
                var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueForApplicationAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);
                var defaultEditionIdValue = await SettingManager.GetSettingValueForApplicationAsync(AppSettings.TenantManagement.DefaultEdition);
                int? defaultEditionId = null;

                if (!string.IsNullOrEmpty(defaultEditionIdValue) && (await _editionManager.FindByIdAsync(Convert.ToInt32(defaultEditionIdValue)) != null))
                {
                    defaultEditionId = Convert.ToInt32(defaultEditionIdValue);
                }

                var tenantId = await TenantManager.CreateWithAdminUserAsync(
                    input.TenancyName,
                    input.TenancyName,
                    input.Password,
                    input.EmailAddress ?? string.Empty,
                    input.PhoneNumber,
                    null,
                    isNewRegisteredTenantActiveByDefault,
                    defaultEditionId,
                    false,
                    true,
                    null,
                    false,
                    _appUrlService.CreateEmailActivationUrlFormat(input.TenancyName)
                );

                var tenant = await TenantManager.GetByIdAsync(tenantId);

                await _appNotifier.NewTenantRegisteredAsync(tenant);

                return new RegisterTenantOutput
                {
                    TenantId = tenant.Id,
                    TenancyName = input.TenancyName,
                    Name = input.TenancyName,
                    UserName = input.TenancyName,
                    IsActive = tenant.IsActive,
                    EmailAddress = input.EmailAddress,
                    IsEmailConfirmationRequired = isEmailConfirmationRequiredForLogin,
                    IsTenantActive = tenant.IsActive
                };
            }
        }

        /// <summary>
        /// 权限重置
        /// </summary>
        /// <returns></returns>
        [UnitOfWork]
        public async Task GrantAllPermissionsAsync()
        {
            using (UnitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                //grant all permissions to admin role
                var adminRoles = _roleManager.Roles.Where(r => r.Name == StaticRoleNames.Tenants.Admin).ToList();

                foreach (var role in adminRoles)
                {
                    await _roleManager.GrantAllPermissionsAsync(role);
                }
            }
        }

        private void CheckTenantRegistrationIsEnabled()
        {
            if (!IsSelfRegistrationEnabled())
            {
                throw new UserFriendlyException(L("SelfTenantRegistrationIsDisabledMessage_Detail"));
            }

            if (!_multiTenancyConfig.IsEnabled)
            {
                throw new UserFriendlyException(L("MultiTenancyIsNotEnabled"));
            }
        }

        private bool IsSelfRegistrationEnabled()
        {
            return SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.AllowSelfRegistration);
        }

        /// <summary>
        /// 使用验证码
        /// </summary>
        /// <returns></returns>
        private bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.UseCaptchaOnRegistration);
        }

        /// <summary>
        /// 检查验证码
        /// </summary>
        /// <returns></returns>
        private async Task CheckCaptcha(RegisterTenantInput input)
        {
            var useCaptchaOnRegistration = SettingManager.GetSettingValueForApplication<bool>(AppSettings.TenantManagement.UseCaptchaOnRegistration);
            if (DebugHelper.IsDebug)
            {
                useCaptchaOnRegistration = false;
            }

            if (!useCaptchaOnRegistration)
                return;

            await _captchaValidator.ValidateAsync(input.CaptchaResponse);
        }
    }
}
