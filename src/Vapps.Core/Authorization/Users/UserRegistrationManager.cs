using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Extensions;
using Abp.Linq;
using Abp.Notifications;
using Abp.Runtime.Session;
using Abp.UI;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Roles;
using Vapps.Configuration;
using Vapps.Debugging;
using Vapps.Editions;
using Vapps.Identity;
using Vapps.MultiTenancy;
using Vapps.Notifications;

namespace Vapps.Authorization.Users
{
    public class UserRegistrationManager : VappsDomainServiceBase
    {
        public IAbpSession AbpSession { get; set; }
        public IAsyncQueryableExecuter AsyncQueryableExecuter { get; set; }

        private readonly TenantManager _tenantManager;
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly EditionManager _editionManager;
        private readonly IUserEmailer _userEmailer;
        private readonly INotificationSubscriptionManager _notificationSubscriptionManager;
        private readonly IAppNotifier _appNotifier;
        private readonly IUserPolicy _userPolicy;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IVerificationCodeManager _verificationCodeManager;

        public UserRegistrationManager(
            TenantManager tenantManager,
            UserManager userManager,
            RoleManager roleManager,
            EditionManager editionManager,
            IUserEmailer userEmailer,
            INotificationSubscriptionManager notificationSubscriptionManager,
            IAppNotifier appNotifier,
            IUserPolicy userPolicy,
            IPasswordHasher<User> passwordHasher,
            IVerificationCodeManager verificationCodeManager)
        {
            _tenantManager = tenantManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _editionManager = editionManager;
            _userEmailer = userEmailer;
            _notificationSubscriptionManager = notificationSubscriptionManager;
            _appNotifier = appNotifier;
            _userPolicy = userPolicy;
            _passwordHasher = passwordHasher;
            _verificationCodeManager = verificationCodeManager;
            AbpSession = NullAbpSession.Instance;

            AsyncQueryableExecuter = NullAsyncQueryableExecuter.Instance;
        }

        /// <summary>
        /// 注册机构用户
        /// </summary>
        /// <param name="tenantName"></param>
        /// <param name="phoneNumber"></param>
        /// <param name="code"></param>
        /// <param name="plainPassword"></param>
        /// <param name="isEmailConfirmed"></param>
        /// <param name="emailActivationLink"></param>
        /// <returns></returns>
        public async Task<User> RegisterWithRelateTenantAsync(string tenantName, string phoneNumber, string code, string plainPassword, bool isEmailConfirmed, string emailActivationLink)
        {
            //await CheckPhoneVerificationCode(tenantName, code);
            await CheckForTenant(tenantName);
            await CheckSelfRegistrationIsEnabled();

            //Create tenant
            var editionId = await _editionManager.GetDefaultEditionIdAsync();
            var tenant = new Tenant(tenantName, tenantName)
            {
                IsActive = true,
                EditionId = editionId,
            };

            await _tenantManager.CreateAsync(tenant);
            await UnitOfWorkManager.Current.SaveChangesAsync(); //To get new tenant's id.

            var user = new User
            {
                Name = tenantName ?? string.Empty,
                Surname = string.Empty,
                EmailAddress = string.Empty,
                IsActive = true,
                UserName = tenantName,
                PhoneNumber = phoneNumber,
                IsEmailConfirmed = isEmailConfirmed,
                Roles = new List<UserRole>(),
            };

            user.SetNormalizedNames();
            user.Password = _passwordHasher.HashPassword(user, plainPassword);

            var defaultRoles = await AsyncQueryableExecuter.ToListAsync(_roleManager.Roles.Where(r => r.IsDefault));
            foreach (var defaultRole in defaultRoles)
            {
                user.Roles.Add(new UserRole(tenant.Id, user.Id, defaultRole.Id));
            }

            CheckErrors(await _userManager.CreateAsync(user));
            await CurrentUnitOfWork.SaveChangesAsync();

            if (!user.IsEmailConfirmed && !user.EmailAddress.IsNullOrWhiteSpace())
            {
                user.SetNewEmailConfirmationCode();
                await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
            }

            //Notifications
            //await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
            await _appNotifier.WelcomeToTheApplicationAsync(user);
            await _appNotifier.NewUserRegisteredAsync(user);

            return user;
        }

        /// <summary>
        /// 注册用户(非机构用户)
        /// </summary>
        /// <param name="name"></param>
        /// <param name="emailAddress"></param>
        /// <param name="userName"></param>
        /// <param name="phone"></param>
        /// <param name="plainPassword"></param>
        /// <param name="isEmailConfirmed"></param>
        /// <param name="isPhoneNumberConfirmed"></param>
        /// <returns></returns>
        public async Task<User> RegisterAsync(string name, string emailAddress, string userName,  string phone, string plainPassword, bool isEmailConfirmed, bool isPhoneNumberConfirmed)
        {
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                //await CheckForTenant(tenantName);
                await CheckSelfRegistrationIsEnabled();

                //var tenant = await GetActiveTenantAsync();
                //var isNewRegisteredUserActiveByDefault = await SettingManager
                //    .GetSettingValueAsync<bool>(AppSettings.UserManagement.IsNewRegisteredUserActiveByDefault);

                var isNewRegisteredUserActiveByDefault = true;

                //await _userPolicy.CheckMaxUserCountAsync(tenant.Id);

                var user = new User
                {
                    //TenantId = tenant.Id,
                    Name = name,
                    Surname = string.Empty,
                    EmailAddress = emailAddress ?? string.Empty,
                    PhoneNumber = phone,
                    IsActive = isNewRegisteredUserActiveByDefault,
                    UserName = userName ,
                    IsEmailConfirmed = isEmailConfirmed,
                    IsPhoneNumberConfirmed = isPhoneNumberConfirmed,
                    Roles = new List<UserRole>(),
                };

                user.SetNormalizedNames();

                foreach (var defaultRole in await _roleManager.Roles.Where(r => r.IsDefault).ToListAsync())
                {
                    user.Roles.Add(new UserRole(null, user.Id, defaultRole.Id));
                }

                await _userManager.InitializeOptionsAsync(AbpSession.TenantId);
                CheckErrors(await _userManager.CreateAsync(user, plainPassword));
                await CurrentUnitOfWork.SaveChangesAsync();

                //if (!user.IsEmailConfirmed && !user.EmailAddress.IsNullOrWhiteSpace())
                //{
                //    user.SetNewEmailConfirmationCode();
                //    await _userEmailer.SendEmailActivationLinkAsync(user, emailActivationLink);
                //}

                //Notifications
                //await _notificationSubscriptionManager.SubscribeToAllAvailableNotificationsAsync(user.ToUserIdentifier());
                await _appNotifier.WelcomeToTheApplicationAsync(user);
                await _appNotifier.NewUserRegisteredAsync(user);

                return user;
            }
        }

        /// <summary>
        /// 租户是否已存在
        /// </summary>
        private async Task CheckForTenant(string name)
        {
            var tenant = await _tenantManager.FindByTenancyNameAsync(name);
            if (tenant != null)
            {
                throw new UserFriendlyException(string.Format(L("Identity.DuplicateTenantName", name)));
            }
        }

        /// <summary>
        /// 检查手机验证码
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task CheckPhoneVerificationCode(string phoneNum, string code)
        {
            if (await _verificationCodeManager.CheckVerificationCodeAsync(code, phoneNum, VerificationCodeType.Register))
            {
                throw new UserFriendlyException(L("InvaildVerificationCode"));
            }
        }

        /// <summary>
        /// 检查注册是否开启
        /// </summary>
        private async Task CheckSelfRegistrationIsEnabled()
        {
            if (!await SettingManager.GetSettingValueAsync<bool>(AppSettings.UserManagement.AllowSelfRegistration))
            {
                throw new UserFriendlyException(L("SelfUserRegistrationIsDisabledMessage_Detail"));
            }
        }

        /// <summary>
        /// 是否在注册中使用验证码
        /// </summary>
        /// <returns></returns>
        private bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        private async Task<Tenant> GetActiveTenantAsync()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return await GetActiveTenantAsync(AbpSession.TenantId.Value);
        }

        private async Task<Tenant> GetActiveTenantAsync(int tenantId)
        {
            var tenant = await _tenantManager.FindByIdAsync(tenantId);
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

        protected virtual void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}
