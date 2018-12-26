using Abp.Authorization;
using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.UI;
using Abp.Zero.Configuration;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Vapps.Authorization.Accounts.Dto;
using Vapps.Authorization.Impersonation;
using Vapps.Authorization.Users;
using Vapps.Configuration;
using Vapps.Debugging;
using Vapps.Identity;
using Vapps.MultiTenancy;
using Vapps.Security.Recaptcha;
using Vapps.Url;

namespace Vapps.Authorization.Accounts
{
    public class AccountAppService : VappsAppServiceBase, IAccountAppService
    {
        private readonly IAppUrlService _appUrlService;
        private readonly ICaptchaValidator _captchaValidator;
        private readonly IUserEmailer _userEmailer;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITenantRegistrationAppService _tenantRegistrationAppService;
        private readonly IVerificationCodeManager _verificationCodeManager;
        private readonly IUserAccountManager _userAccountManager;
        private readonly UserManager _userManager;
        private readonly UserRegistrationManager _userRegistrationManager;

        public AccountAppService(
            IUserEmailer userEmailer,
            IImpersonationManager impersonationManager,
            IUserLinkManager userLinkManager,
            ICaptchaValidator captchaValidator,
            IAppUrlService appUrlService,
            IPasswordHasher<User> passwordHasher,
            ITenantRegistrationAppService tenantRegistrationAppService,
            IVerificationCodeManager verificationCodeManager,
            IUserAccountManager userAccountManager,
            UserManager userManager,
            UserRegistrationManager userRegistrationManager)
        {
            _userEmailer = userEmailer;
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _appUrlService = appUrlService;
            _captchaValidator = captchaValidator;
            _passwordHasher = passwordHasher;
            _tenantRegistrationAppService = tenantRegistrationAppService;
            _verificationCodeManager = verificationCodeManager;
            _userManager = userManager;
            _userAccountManager = userAccountManager;
        }

        #region Methods

        /// <summary>
        /// 租户是否可用
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<IsTenantAvailableOutput> IsTenantAvailable(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.NotFound);
            }

            if (!tenant.IsActive)
            {
                return new IsTenantAvailableOutput(TenantAvailabilityState.InActive);
            }

            return new IsTenantAvailableOutput(TenantAvailabilityState.Available, tenant.Id);
        }

        /// <summary>
        /// 解析租户Id
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Task<int?> ResolveTenantId(ResolveTenantIdInput input)
        {
            if (string.IsNullOrEmpty(input.c))
            {
                return Task.FromResult(AbpSession.TenantId);
            }

            var parameters = SimpleStringCipher.Instance.Decrypt(input.c);
            var query = HttpUtility.ParseQueryString(parameters);

            if (query["tenantId"] == null)
            {
                throw new Exception("Couldn't find tenant inforamtion !");
            }

            var tenantId = Convert.ToInt32(query["tenantId"]) as int?;
            return Task.FromResult(tenantId);
        }

        /// <summary>
        /// 租户是否存在
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<bool> IsTenantNameExist(IsTenantAvailableInput input)
        {
            var tenant = await TenantManager.FindByTenancyNameAsync(input.TenancyName);
            if (tenant == null)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 注册
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<RegisterOutput> Register(RegisterInput input)
        {
            using (CurrentUnitOfWork.SetTenantId(null))
            {
                //if (UseCaptchaOnRegistration()) //使用手机验证码作为验证
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

                var randomPassword = User.CreateRandomPassword();

                var user = await _userRegistrationManager.RegisterAsync(
                input.Name ?? string.Empty,
                input.EmailAddress ?? string.Empty,
                StringExtensions.GetRandomString(12),
                input.PhoneNumber,
                randomPassword,
                input.Type == RegisterType.Email,
                input.Type == RegisterType.Telephone);

                var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

                return new RegisterOutput
                {
                    CanLogin = user.IsActive && (user.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin),
                    RandomPassword = user.IsActive ? randomPassword : null
                };
            }
        }

        /// <summary>
        /// 发送密码重置邮箱
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendPasswordResetCode(SendPasswordResetCodeInput input)
        {
            var user = await GetUserByChecking(input.EmailAddress);
            user.SetNewPasswordResetCode();
            await _userEmailer.SendPasswordResetLinkAsync(
                user,
                _appUrlService.CreatePasswordResetUrlFormat(AbpSession.TenantId)
                );
        }

        /// <summary>
        /// 重置密码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ResetPasswordOutput> ResetPassword(ResetPasswordInput input)
        {
            var user = await UserManager.UserStore.Find4PlatformByIdAsync(input.UserId);
            if (user == null || user.PasswordResetCode.IsNullOrEmpty() || user.PasswordResetCode != input.ResetCode)
            {
                throw new UserFriendlyException(L("InvalidPasswordResetCode"), L("InvalidPasswordResetCode_Detail"));
            }

            CheckErrors(await UserManager.ChangePasswordAsync(user, input.Password));
            user.PasswordResetCode = null;
            user.IsEmailConfirmed = true;
            user.ShouldChangePasswordOnNextLogin = false;

            await UserManager.UpdateAsync(user);

            return new ResetPasswordOutput
            {
                CanLogin = user.IsActive,
                UserName = user.UserName
            };
        }

        /// <summary>
        /// 发送邮箱验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendEmailVerificationCode(SendEmailVerificationCodeInput input)
        {
            await CheckEmailAddressVaild(input);

            var verificationCode = await _verificationCodeManager.GetOrSetVerificationCodeAsync(input.EmailAddress, input.CodeType);

            try
            {
                await _userEmailer
                    .SendEmailVerificationCodeAsync(input.EmailAddress, verificationCode.Code, input.CodeType);
            }
            catch (SmtpCommandException)
            {
                throw new UserFriendlyException(L("RecipientNotExisted"));
            }
        }

        /// <summary>
        /// 验证当前用户的验证码
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CheckEmailCodeByCurrentUser(CheckEmailCodeInput input)
        {
            //验证当前用户的手机验证码
            var user = await GetCurrentUserAsync();
            var result = await _verificationCodeManager.CheckVerificationCodeAsync(input.Code, user.EmailAddress, input.CodeType);
            if (!result)
                throw new UserFriendlyException(L("InvaildVerificationCode"));
        }

        /// <summary>
        /// 发送激活邮件
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task SendEmailActivationLink(SendEmailActivationLinkInput input)
        {
            var user = await GetUserByChecking(input.EmailAddress);
            if (user.EmailAddress.IsNullOrWhiteSpace()) return;

            user.SetNewEmailConfirmationCode();
            await _userEmailer
                .SendEmailActivationLinkAsync(user, _appUrlService.CreateEmailActivationUrlFormat(AbpSession.TenantId));
        }

        /// <summary>
        /// 邮箱激活
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ActivateEmail(ActivateEmailInput input)
        {
            var user = await UserManager.GetUserByIdAsync(input.UserId);
            if (user == null || user.EmailConfirmationCode.IsNullOrEmpty() || user.EmailConfirmationCode != input.ConfirmationCode)
            {
                throw new UserFriendlyException(L("InvalidEmailConfirmationCode"), L("InvalidEmailConfirmationCode_Detail"));
            }

            user.IsEmailConfirmed = true;
            user.EmailConfirmationCode = null;

            await UserManager.UpdateAsync(user);
        }

        /// <summary>
        /// 模拟(用户)登录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.UserManage.Users.Impersonation)]
        public virtual async Task<ImpersonateOutput> Impersonate(ImpersonateInput input)
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetImpersonationToken(input.UserId, input.TenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TenantId)
            };
        }

        /// <summary>
        /// 退出模拟登录
        /// </summary>
        /// <returns></returns>
        public virtual async Task<ImpersonateOutput> BackToImpersonator()
        {
            return new ImpersonateOutput
            {
                ImpersonationToken = await _impersonationManager.GetBackToImpersonatorToken(),
                TenancyName = await GetTenancyNameOrNullAsync(AbpSession.ImpersonatorTenantId)
            };
        }

        /// <summary>
        /// 关联账号
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<SwitchToLinkedAccountOutput> SwitchToLinkedAccount(SwitchToLinkedAccountInput input)
        {
            if (!await _userLinkManager.AreUsersLinked(AbpSession.ToUserIdentifier(), input.ToUserIdentifier()))
            {
                throw new Exception(L("This account is not linked to your account"));
            }

            return new SwitchToLinkedAccountOutput
            {
                SwitchAccountToken = await _userLinkManager.GetAccountSwitchToken(input.TargetUserId, input.TargetTenantId),
                TenancyName = await GetTenancyNameOrNullAsync(input.TargetTenantId)
            };
        }




        #endregion

        #region Utilities

        private bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await TenantManager.FindByIdAsync(tenantId);
            if (tenant == null)
            {
                throw new UserFriendlyException(L("UnknownTenantId{0}", tenantId));
            }

            if (!tenant.IsActive)
            {
                throw new UserFriendlyException(L("TenantIdIsNotActive{0}", tenantId));
            }

            return tenant;
        }

        private async Task<string> GetTenancyNameOrNullAsync(int? tenantId)
        {
            return tenantId.HasValue ? (await GetActiveTenantAsync(tenantId.Value)).TenancyName : null;
        }

        private async Task<User> GetUserByChecking(string inputEmailAddress)
        {
            var user = await UserManager.UserStore.Find4PlatformByEmailAsync(inputEmailAddress);
            if (user == null)
            {
                throw new UserFriendlyException(L("InvalidEmailAddress"));
            }

            return user;
        }

        /// <summary>
        /// 验证手机号码是否有效
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        private async Task CheckEmailAddressVaild(SendEmailVerificationCodeInput input)
        {
            if (input.CodeType == VerificationCodeType.Register)
            {
                var user = await _userManager.UserStore.Find4PlatformByEmailAsync(input.EmailAddress);
                if (user != null)
                    throw new UserFriendlyException(string.Format(L("Identity.DuplicateEmail"), input.EmailAddress));
            }
            else if (input.CodeType == VerificationCodeType.EmailBinding)
            {
                var currentUser = await GetCurrentUserIfLoginAsync();
                var user = await _userManager.UserStore.Find4PlatformByEmailAsync(input.EmailAddress);

                //未登录，不能收取邮箱绑定验证码
                if (currentUser == null)
                    throw new UserFriendlyException(L("Identity.NotLoggedIn"));

                if (user != null)
                {
                    if (user != currentUser)
                        throw new UserFriendlyException(string.Format(L("Identity.DuplicateEmail"), input.EmailAddress));

                    if (user == currentUser)
                        throw new UserFriendlyException(string.Format(L("Identity.BindingEmailCanNotEqual"), input.EmailAddress));
                }
            }
            else if (input.CodeType == VerificationCodeType.EmailUnBinding)
            {
                var currentUser = await GetCurrentUserIfLoginAsync();
                if (currentUser == null || currentUser.EmailAddress.IsNullOrWhiteSpace())
                {
                    throw new UserFriendlyException(string.Format(L("Identity.UnBindingEmail"), input.EmailAddress));
                }
            }
        }

        #endregion

    }
}