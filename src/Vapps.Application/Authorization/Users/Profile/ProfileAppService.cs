using Abp;
using Abp.Auditing;
using Abp.Authorization;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using System;
using System.Threading.Tasks;
using Vapps.Authorization.Accounts;
using Vapps.Authorization.Accounts.Cache;
using Vapps.Authorization.Users.Dto;
using Vapps.Authorization.Users.Profile.Dto;
using Vapps.Helpers;
using Vapps.Identity;
using Vapps.Media;
using Vapps.Security;
using Vapps.Storage;
using Vapps.Timing;

namespace Vapps.Authorization.Users.Profile
{
    [AbpAuthorize]
    public class ProfileAppService : VappsAppServiceBase, IProfileAppService
    {
        private readonly IAppFolders _appFolders;
        private readonly IBinaryObjectManager _binaryObjectManager;
        private readonly ITimeZoneService _timeZoneService;
        private readonly IPictureManager _pictureManager;
        private readonly IVerificationCodeManager _verificationCodeManager;
        private readonly IUserAccountManager _userAccountManager;
        private readonly IAccountCache _accountCache;
        private readonly UserManager _userManager;

        public ProfileAppService(
            IAppFolders appFolders,
            IBinaryObjectManager binaryObjectManager,
            ITimeZoneService timezoneService,
            IPictureManager pictureManager,
            IVerificationCodeManager verificationCodeManager,
            IUserAccountManager userAccountManager,
            IAccountCache accountCache,
            UserManager userManager)
        {
            _appFolders = appFolders;
            _binaryObjectManager = binaryObjectManager;
            _timeZoneService = timezoneService;
            _pictureManager = pictureManager;
            _verificationCodeManager = verificationCodeManager;
            _userAccountManager = userAccountManager;
            _accountCache = accountCache;
            _userManager = userManager;
        }

        #region Methods

        /// <summary>
        /// 获取当前用户资料
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<CurrentUserProfileEditDto> GetCurrentUserProfileForEdit()
        {
            var user = await GetCurrentUserAsync();
            var userAccount = await GetCurrentUserAccountCacheAsync();
            var userProfileEditDto = ObjectMapper.Map<CurrentUserProfileEditDto>(user);
            userProfileEditDto.ProfilePictureUrl = await _pictureManager.GetPictureUrlAsync(userAccount.ProfilePictureId);
            userProfileEditDto.NickName = userAccount.NickName;
            userProfileEditDto.Gender = userAccount.Gender;
            userProfileEditDto.PhoneNumber = user.PhoneNumber;
            if (Clock.SupportsMultipleTimezone)
            {
                userProfileEditDto.Timezone = await SettingManager.GetSettingValueAsync(TimingSettingNames.TimeZone);

                var defaultTimeZoneId = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                if (userProfileEditDto.Timezone == defaultTimeZoneId)
                {
                    userProfileEditDto.Timezone = string.Empty;
                }
            }

            return userProfileEditDto;
        }

        /// <summary>
        /// 更新当前用户资料
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateCurrentUserProfile(CurrentUserProfileEditDto input)
        {
            var user = await GetCurrentUserAsync();
            var userAccount = await _userAccountManager.GetByUserIdAsync(AbpSession.GetUserId());
            ObjectMapper.Map(input, user);
            CheckErrors(await UserManager.Update4PlatformAsync(user));
            userAccount.NickName = input.NickName;
            userAccount.Gender = input.Gender;

            if (Clock.SupportsMultipleTimezone)
            {
                if (input.Timezone.IsNullOrEmpty())
                {
                    var defaultValue = await _timeZoneService.GetDefaultTimezoneAsync(SettingScopes.User, AbpSession.TenantId);
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, defaultValue);
                }
                else
                {
                    await SettingManager.ChangeSettingForUserAsync(AbpSession.ToUserIdentifier(), TimingSettingNames.TimeZone, input.Timezone);
                }
            }
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ChangePassword(ChangePasswordInput input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await GetCurrentUserAsync();
            CheckErrors(await UserManager.ChangePasswordAsync(user, input.CurrentPassword, input.NewPassword));
        }

        /// <summary>
        /// 使用手机修改密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ChangePasswordByPhone(ChangePasswordByPhoneInput input)
        {
            await UserManager.InitializeOptionsAsync(AbpSession.TenantId);

            var user = await GetCurrentUserAsync();
            var result = await _verificationCodeManager.CheckVerificationCodeAsync(input.Code, user.PhoneNumber, VerificationCodeType.ChangePassword);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            CheckErrors(await UserManager.ChangePasswordAsync(user, input.NewPassword));
        }

        /// <summary>
        /// 绑定手机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BindingPhoneNum(BindingPhoneNumInput input)
        {
            var user = await GetCurrentUserAsync();
            if (!user.PhoneNumber.IsNullOrEmpty())
                throw new UserFriendlyException(L("Identity.HadBindingPhoneNum"));

            var result = await _verificationCodeManager.CheckVerificationCodeAsync(input.Code, input.PhoneNum, VerificationCodeType.PhoneBinding);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            user.PhoneNumber = input.PhoneNum;
        }

        /// <summary>
        /// 修改绑定手机
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ChangeBindingPhoneNum(ChangeBindingPhoneNumInput input)
        {
            var user = await GetCurrentUserAsync();
            if (user.PhoneNumber.IsNullOrEmpty())
                throw new UserFriendlyException(L("Identity.UnBindingPhoneNum"));

            //验证旧手机验证码
            var result = await _verificationCodeManager.CheckVerificationCodeAsync(input.ValidCode, user.PhoneNumber, VerificationCodeType.PhoneUnBinding, true);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            //验证新手机验证
            result = await _verificationCodeManager.CheckVerificationCodeAsync(input.BundlingCode, input.NewTelephone, VerificationCodeType.PhoneBinding);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            user.PhoneNumber = input.NewTelephone;

            CheckErrors(await UserManager.Update4PlatformAsync(user));
        }

        /// <summary>
        /// 解绑手机
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task UnBindingPhoneNum(string code)
        {
            var user = await GetCurrentUserAsync();
            if (user.PhoneNumber.IsNullOrEmpty())
                throw new UserFriendlyException(L("Identity.UnBindingPhoneNum"));

            var result = await _verificationCodeManager.CheckVerificationCodeAsync(code, user.PhoneNumber, VerificationCodeType.PhoneUnBinding);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            user.PhoneNumber = null;
        }

        /// <summary>
        /// 更新头像
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateProfilePicture(UpdateProfilePictureInput input)
        {
            var userAccount = await _userAccountManager.GetByUserIdAsync(AbpSession.GetTenantId(), AbpSession.GetUserId());

            if (userAccount.ProfilePictureId > 0)
            {
                await _pictureManager.DeleteAsync(userAccount.ProfilePictureId);
            }

            userAccount.ProfilePictureId = input.ProfilePictureId;
        }

        /// <summary>
        /// 获取密码复杂性
        /// </summary>
        /// <returns></returns>
        [AbpAllowAnonymous]
        public async Task<GetPasswordComplexitySettingOutput> GetPasswordComplexitySetting()
        {
            var passwordComplexitySetting = new PasswordComplexitySetting
            {
                RequireDigit = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireDigit),
                RequireLowercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireLowercase),
                RequireNonAlphanumeric = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireNonAlphanumeric),
                RequireUppercase = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequireUppercase),
                RequiredLength = await SettingManager.GetSettingValueAsync<int>(AbpZeroSettingNames.UserManagement.PasswordComplexity.RequiredLength)
            };

            return new GetPasswordComplexitySettingOutput
            {
                Setting = passwordComplexitySetting
            };
        }

        /// <summary>
        /// 获取当前用户头像
        /// </summary>
        /// <returns></returns>
        [DisableAuditing]
        public async Task<GetProfilePictureOutput> GetProfilePicture()
        {
            var userAccount = await _accountCache.GetAsync(AbpSession.GetUserId());

            if (userAccount == null || userAccount.ProfilePictureId <= 0)
            {
                return new GetProfilePictureOutput(string.Empty);
            }

            return await GetProfilePictureById(userAccount.ProfilePictureId);
        }

        /// <summary>
        /// 根据 Id 获取头像
        /// </summary>
        /// <param name="profilePictureId">头像文件Id</param>
        /// <returns></returns>
        public async Task<GetProfilePictureOutput> GetProfilePictureById(long profilePictureId)
        {
            return await GetProfilePictureByIdInternal(profilePictureId);
        }

        /// <summary>
        /// 修改语言
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ChangeLanguage(ChangeUserLanguageDto input)
        {
            await SettingManager.ChangeSettingForUserAsync(
                    AbpSession.ToUserIdentifier(),
                    LocalizationSettingNames.DefaultLanguage,
                    input.LanguageName
                );
        }

        /// <summary>
        /// 获取当前用户安全信息
        /// </summary>
        /// <returns></returns>
        public async Task<UserSecurityInfoDto> GetCurrentUserSecurityInfo()
        {
            var user = await GetCurrentUserAsync();

            var result = new UserSecurityInfoDto()
            {
                EmailAddress = user.EmailAddress,
                PhoneNumber = user.PhoneNumber,
            };

            var weChatExternalLogin = await _userManager.UserStore.GetAssociateExternal(await GetCurrentUserAsync(), VappsConsts.WeChatLogin);
            if (weChatExternalLogin != null)
                result.WeChat = weChatExternalLogin.ExternalDisplayName;

            var qqExternalLogin = await _userManager.UserStore.GetAssociateExternal(await GetCurrentUserAsync(), VappsConsts.QQLogin);
            if (qqExternalLogin != null)
                result.QQ = qqExternalLogin.ExternalDisplayName;

            return result;
        }

        /// <summary>
        /// 绑定邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BindingEmailAddress(BindingEmailInput input)
        {
            var user = await GetCurrentUserAsync();
            if (!user.EmailAddress.IsNullOrEmpty())
                throw new UserFriendlyException(L("Identity.HadBindingEmail"));

            var result = await _verificationCodeManager.CheckVerificationCodeAsync(input.Code, input.EmailAddress, VerificationCodeType.EmailBinding);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            user.EmailAddress = input.EmailAddress;
            user.SetNormalizedNames();
        }

        /// <summary>
        /// 修改绑定邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ChangeBindingEmail(ChangeBindingEmailInput input)
        {
            var user = await GetCurrentUserAsync();
            if (user.EmailAddress.IsNullOrEmpty())
                throw new UserFriendlyException(L("Identity.UnBindingEmail"));

            //验证旧邮箱验证码
            var result = await _verificationCodeManager.CheckVerificationCodeAsync(input.ValidCode, user.EmailAddress, VerificationCodeType.EmailUnBinding, true);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            //验证新邮箱验证码
            result = await _verificationCodeManager.CheckVerificationCodeAsync(input.BindlingCode, input.NewEmailAddress, VerificationCodeType.EmailBinding);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            user.EmailAddress = input.NewEmailAddress;
            user.SetNormalizedNames();
            CheckErrors(await UserManager.Update4PlatformAsync(user));
        }

        /// <summary>
        /// 解绑邮箱
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task UnBindingEmailAddress(string code)
        {
            var user = await GetCurrentUserAsync();
            if (user.EmailAddress.IsNullOrEmpty())
                throw new UserFriendlyException(L("Identity.UnBindingEmail"));

            var result = await _verificationCodeManager.CheckVerificationCodeAsync(code, user.EmailAddress, VerificationCodeType.EmailUnBinding);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));

            user.EmailAddress = string.Empty;
            user.SetNormalizedNames();
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 根据Id 获取头像
        /// </summary>
        /// <param name="profilePictureId"></param>
        /// <returns></returns>
        private async Task<byte[]> GetProfilePictureByIdOrNull(long profilePictureId)
        {
            var fileUrl = await _pictureManager.GetPictureUrlAsync(profilePictureId);
            if (fileUrl.IsNullOrWhiteSpace())
            {
                return null;
            }

            var bytes = await CommonHelper.SavePictureFromUrlAsync(fileUrl);

            return bytes;
        }

        /// <summary>
        /// 根据Id获取用户头像
        /// </summary>
        /// <param name="profilePictureId"></param>
        /// <returns></returns>
        private async Task<GetProfilePictureOutput> GetProfilePictureByIdInternal(long profilePictureId)
        {
            var pictureUrl = await _pictureManager.GetPictureUrlAsync(profilePictureId);

            return new GetProfilePictureOutput(pictureUrl);
        }

        #endregion
    }
}