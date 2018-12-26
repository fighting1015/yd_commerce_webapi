using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Localization;
using Abp.Localization.Sources;
using Abp.UI;
using Abp.Zero.Configuration;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.ExternalAuthentications;
using Vapps.Identity;
using Vapps.MultiTenancy;

namespace Vapps.Authorization.Accounts
{
    public class LogInManager : AbpLogInManager<Tenant, Role, User>
    {
        private readonly UserManager _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IVerificationCodeManager _verificationCodeManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ILogger _logger;

        public LogInManager(
            UserManager userManager,
            IMultiTenancyConfig multiTenancyConfig,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ISettingManager settingManager,
            IRepository<UserLoginAttempt, long> userLoginAttemptRepository,
            IUserManagementConfig userManagementConfig,
            IIocResolver iocResolver,
            RoleManager roleManager,
            IPasswordHasher<User> passwordHasher,
            UserClaimsPrincipalFactory claimsPrincipalFactory,
            IVerificationCodeManager verificationCodeManager,
            ILocalizationManager localizationManager,
            ILogger logger)
            : base(
                  userManager,
                  multiTenancyConfig,
                  tenantRepository,
                  unitOfWorkManager,
                  settingManager,
                  userLoginAttemptRepository,
                  userManagementConfig,
                  iocResolver,
                  passwordHasher,
                  roleManager,
                  claimsPrincipalFactory)
        {
            this._localizationManager = localizationManager;
            this._userManager = userManager;
            this._passwordHasher = passwordHasher;
            this._verificationCodeManager = verificationCodeManager;
            this._logger = logger;
        }

        #region Account login

        /// <summary>
        /// 账号密码登陆
        /// </summary>
        /// <param name="loginCertificate">登陆凭证</param>
        /// <param name="plainPassword">密码</param>
        /// <param name="tenancyName">租户名称</param>
        /// <param name="shouldLockout">是否锁定</param>
        /// <returns></returns>
        [UnitOfWork]
        public override async Task<AbpLoginResult<Tenant, User>> LoginAsync(string loginCertificate,
            string plainPassword, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginAsyncInternal(loginCertificate, plainPassword, tenancyName, shouldLockout);
            await SaveLoginAttempt(result, tenancyName, loginCertificate);
            return result;
        }

        /// <summary>
        /// 账号密码登陆
        /// </summary>
        /// <param name="loginCertificate">登陆凭证</param>
        /// <param name="plainPassword">密码</param>
        /// <param name="tenancyName">租户名称</param>
        /// <param name="shouldLockout">是否锁定</param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<AbpLoginResult<Tenant, User>> LoginWithPhoneCodeAsync(string phoneNum,
            string code, string tenancyName = null, bool shouldLockout = true)
        {
            var result = await LoginWithPhoneNumAsync(phoneNum, code, tenancyName, shouldLockout);
            await SaveLoginAttempt(result, tenancyName, phoneNum);
            return result;
        }

        /// <summary>
        /// 统一逻辑(统一登录接口)
        /// </summary>
        /// <param name="loginCertificate">登陆凭证(用户名/邮箱/手机)</param>
        /// <param name="plainPassword">密码</param>
        /// <param name="tenancyName">租户名称</param>
        /// <param name="shouldLockout">是否锁定</param>
        /// <returns></returns>
        protected override async Task<AbpLoginResult<Tenant, User>> LoginAsyncInternal(string loginCertificate,
            string plainPassword, string tenancyName, bool shouldLockout)
        {
            if (loginCertificate.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(loginCertificate));
            }

            if (plainPassword.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(plainPassword));
            }

            //Get and check tenant
            var user = await _userManager.UserStore.FindByLoginCertificate4PlatformAsync(loginCertificate);
            if (user == null)
            {
                return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidUserNameOrEmailAddress);
            }

            Tenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (user.TenantId.HasValue)
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.Id == user.TenantId);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidTenancyName);
                    }

                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                /* TryLoginFromExternalAuthenticationSources 方法可能会创建用户
                 * 因此我们需要在 FindByLoginCertificateAsync 前调用
                 */
                var loggedInFromExternalSource = await TryLoginFromExternalAuthenticationSources(loginCertificate, plainPassword, tenant);

                if (!loggedInFromExternalSource)
                {
                    if (await UserManager.IsLockedOutAsync(user))
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                    }

                    if (!await UserManager.CheckPasswordAsync(user, plainPassword))
                    {
                        if (shouldLockout)
                        {
                            if (await TryLockOutAsync(tenantId, user.Id))
                            {
                                return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                            }
                        }

                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidPassword, tenant, user);
                    }

                    await UserManager.ResetAccessFailedCountAsync(user);
                }

                return await CreateLoginResultAsync(user, tenant);
            }
        }

        /// <summary>
        /// 统一逻辑(统一登录接口)
        /// </summary>
        /// <param name="phoneNum">手机号码</param>
        /// <param name="code">手机验证码</param>
        /// <param name="tenancyName">租户名称</param>
        /// <param name="shouldLockout">是否锁定</param>
        /// <returns></returns>
        protected async Task<AbpLoginResult<Tenant, User>> LoginWithPhoneNumAsync(string phoneNum,
            string code, string tenancyName, bool shouldLockout)
        {
            if (phoneNum.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(phoneNum));
            }

            if (code.IsNullOrEmpty())
            {
                throw new ArgumentNullException(nameof(code));
            }

            //Get and check tenant
            var user = await _userManager.UserStore.Find4PlatformByPhoneNumberAsync(phoneNum);
            if (user == null)
            {
                return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidUserNameOrEmailAddress);
            }

            Tenant tenant = null;
            using (UnitOfWorkManager.Current.SetTenantId(null))
            {
                if (!MultiTenancyConfig.IsEnabled)
                {
                    tenant = await GetDefaultTenantAsync();
                }
                else if (user.TenantId.HasValue)
                {
                    tenant = await TenantRepository.FirstOrDefaultAsync(t => t.Id == user.TenantId);
                    if (tenant == null)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidTenancyName);
                    }
                    if (!tenant.IsActive)
                    {
                        return new AbpLoginResult<Tenant, User>(AbpLoginResultType.TenantIsNotActive, tenant);
                    }
                }
            }

            var tenantId = tenant == null ? (int?)null : tenant.Id;
            if (!await _verificationCodeManager.CheckVerificationCodeAsync(code, phoneNum, VerificationCodeType.Login))
            {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    if (shouldLockout)
                    {
                        if (await TryLockOutAsync(tenantId, user.Id))
                        {
                            return new AbpLoginResult<Tenant, User>(AbpLoginResultType.LockedOut, tenant, user);
                        }
                    }
                }

                throw new UserFriendlyException(L("LoginFailed"), L("InvaildVerificationCode"));
            }

            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                await UserManager.ResetAccessFailedCountAsync(user);
                return await CreateLoginResultAsync(user, tenant);
            }
        }


        /// <summary>
        /// 外部认证源登陆
        /// </summary>
        /// <param name="loginCertificate"></param>
        /// <param name="plainPassword"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        protected override async Task<bool> TryLoginFromExternalAuthenticationSources(string loginCertificate, string plainPassword, Tenant tenant)
        {
            if (!UserManagementConfig.ExternalAuthenticationSources.Any())
            {
                return false;
            }

            foreach (var sourceType in UserManagementConfig.ExternalAuthenticationSources)
            {
                using (var source = IocResolver.ResolveAsDisposable<IExternalAuthenticationSource<Tenant, User>>(sourceType))
                {
                    if (await source.Object.TryAuthenticateAsync(loginCertificate, plainPassword, tenant))
                    {
                        var tenantId = tenant == null ? (int?)null : tenant.Id;
                        using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                        {
                            var user = await _userManager.UserStore.FindByLoginCertificate4PlatformAsync(loginCertificate);
                            if (user == null)
                            {
                                user = await source.Object.CreateUserAsync(loginCertificate, tenant);

                                user.TenantId = tenantId;
                                user.AuthenticationSource = source.Object.Name;
                                user.Password = _passwordHasher.HashPassword(user, Guid.NewGuid().ToString("N").Left(16)); //Setting a random password since it will not be used

                                if (user.Roles == null)
                                {
                                    user.Roles = new List<UserRole>();
                                    foreach (var defaultRole in RoleManager.Roles.Where(r => r.TenantId == tenantId && r.IsDefault).ToList())
                                    {
                                        user.Roles.Add(new UserRole(tenantId, user.Id, defaultRole.Id));
                                    }
                                }

                                await _userManager.UserStore.CreateAsync(user);
                            }
                            else
                            {
                                await source.Object.UpdateUserAsync(user, tenant);

                                user.AuthenticationSource = source.Object.Name;

                                await _userManager.UserStore.UpdateAsync(user);
                            }

                            await UnitOfWorkManager.Current.SaveChangesAsync();

                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 直接生成登录凭证(慎用)
        /// </summary>
        /// <param name="user"></param>
        /// <param name="tenant"></param>
        /// <returns></returns>
        public virtual Task<AbpLoginResult<Tenant, User>> ImmediateCreateLoginResultAsync(User user, Tenant tenant = null)
        {
            return CreateLoginResultAsync(user, tenant);
        }

        #endregion

        #region  External user login

        [UnitOfWork]
        public virtual async Task<AbpLoginResult<Tenant, User>> LoginAsync(ExternalUserLoginInfo login)
        {
            var result = await LoginAsyncInternal(login);
            _logger.Debug(ClientInfoProvider.BrowserInfo);
            _logger.Debug(ClientInfoProvider.ClientIpAddress);
            _logger.Debug(ClientInfoProvider.ComputerName);

            await SaveLoginAttempt(result, result.Tenant?.Name ?? null, login.ProviderKey + "@" + login.LoginProvider);
            return result;
        }

        protected virtual async Task<AbpLoginResult<Tenant, User>> LoginAsyncInternal(ExternalUserLoginInfo login)
        {
            if (login == null || login.LoginProvider.IsNullOrEmpty() || login.ProviderKey.IsNullOrEmpty())
            {
                throw new ArgumentException("login");
            }

            //Get and check tenant
            var user = await _userManager.UserStore.Find4PlatformAsync(login);
            if (user == null)
            {
                return new AbpLoginResult<Tenant, User>(AbpLoginResultType.UnknownExternalLogin);
            }

            //Get and check tenant
            Tenant tenant = null;
            if (!MultiTenancyConfig.IsEnabled)
            {
                tenant = await GetDefaultTenantAsync();
            }
            else if (user.TenantId.HasValue)
            {
                tenant = await TenantRepository.FirstOrDefaultAsync(t => t.Id == user.TenantId);
                if (tenant == null)
                {
                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.InvalidTenancyName);
                }

                if (!tenant.IsActive)
                {
                    return new AbpLoginResult<Tenant, User>(AbpLoginResultType.TenantIsNotActive, tenant);
                }
            }

            int? tenantId = tenant == null ? (int?)null : tenant.Id;
            using (UnitOfWorkManager.Current.SetTenantId(tenantId))
            {
                return await CreateLoginResultAsync(user, tenant);
            }
        }

        /// <summary>
        /// Gets localized string for given key name and current language.
        /// </summary>
        /// <param name="name">Key name</param>
        /// <returns>Localized string</returns>
        protected virtual string L(string name)
        {
            return _localizationManager.GetString(VappsConsts.ServerSideLocalizationSourceName, name);
        }

        #endregion
    }
}