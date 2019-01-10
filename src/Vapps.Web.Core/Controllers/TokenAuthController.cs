using Abp;
using Abp.AspNetCore.Mvc.Authorization;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Configuration;
using Abp.Domain.Repositories;
using Abp.Events.Bus;
using Abp.Extensions;
using Abp.MultiTenancy;
using Abp.Net.Mail;
using Abp.Notifications;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Abp.Runtime.Session;
using Abp.Timing;
using Abp.UI;
using Abp.Zero.Configuration;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Vapps.Authorization;
using Vapps.Authorization.Accounts;
using Vapps.Authorization.Impersonation;
using Vapps.Authorization.Users;
using Vapps.Configuration;
using Vapps.Debugging;
using Vapps.Editions;
using Vapps.Editions.Cache;
using Vapps.Enums;
using Vapps.ExternalAuthentications;
using Vapps.Identity;
using Vapps.MultiTenancy;
using Vapps.Notifications;
using Vapps.Security.Recaptcha;
using Vapps.SMS;
using Vapps.Url;
using Vapps.Web.Authentication.External;
using Vapps.Web.Authentication.External.QQ;
using Vapps.Web.Authentication.External.Wechat;
using Vapps.Web.Authentication.JwtBearer;
using Vapps.Web.Authentication.TwoFactor;
using Vapps.Web.Models.TokenAuth;

namespace Vapps.Web.Controllers
{
    /// <summary>
    /// 用户认证
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class TokenAuthController : VappsControllerBase
    {
        private const string UserIdentifierClaimType = "http://vapps.com/claims/useridentifier";

        private readonly IAppUrlService _appUrlService;
        private readonly IAppNotifier _appNotifier;
        private readonly ICacheManager _cacheManager;
        private readonly IEmailSender _emailSender;
        private readonly IEventBus _eventBus;
        private readonly IExternalAuthConfiguration _externalAuthConfiguration;
        private readonly IExternalAuthenticationSettingStore _externalAuthenticationSettingStore;
        private readonly IExternalAuthManager _externalAuthManager;
        private readonly IImpersonationManager _impersonationManager;
        private readonly IOptions<JwtBearerOptions> _jwtOptions;
        private readonly ISmsSender _smsSender;
        private readonly ITenantCache _tenantCache;
        private readonly IUserLinkManager _userLinkManager;
        private readonly IVerificationCodeManager _verificationCodeManager;
        private readonly ISubscribableEditionCache _editionCache;

        private readonly AbpLoginResultTypeHelper _abpLoginResultTypeHelper;
        private readonly ExternalAuthenticationSetting _externalsettings;
        private readonly LogInManager _logInManager;
        private readonly IdentityOptions _identityOptions;
        private readonly TokenAuthConfiguration _configuration;
        private readonly UserManager _userManager;
        private readonly UserRegistrationManager _userRegistrationManager;
        private readonly TenantManager _tenantManager;

        public TokenAuthController(
            IAppUrlService appUrlService,
            IAppNotifier appNotifier,
            ICacheManager cacheManager,
            IEmailSender emailSender,
            IEventBus eventBus,
            IExternalAuthConfiguration externalAuthConfiguration,
            IExternalAuthenticationSettingStore externalAuthenticationSettingStore,
            IExternalAuthManager externalAuthManager,
            IImpersonationManager impersonationManager,
            IOptions<IdentityOptions> identityOptions,
            IOptions<JwtBearerOptions> jwtOptions,
            ISmsSender smsSender,
            ITenantCache tenantCache,
            IUserLinkManager userLinkManager,
            IVerificationCodeManager verificationCodeManager,
            ISubscribableEditionCache editionCache,
            AbpLoginResultTypeHelper abpLoginResultTypeHelper,
            LogInManager logInManager,
            TokenAuthConfiguration configuration,
            UserManager userManager,
            UserRegistrationManager userRegistrationManager,
            TenantManager tenantManager)
        {
            _appUrlService = appUrlService;
            _logInManager = logInManager;
            _tenantCache = tenantCache;
            _abpLoginResultTypeHelper = abpLoginResultTypeHelper;
            _configuration = configuration;
            _userManager = userManager;
            _cacheManager = cacheManager;
            _jwtOptions = jwtOptions;
            _externalAuthConfiguration = externalAuthConfiguration;
            _externalAuthManager = externalAuthManager;
            _userRegistrationManager = userRegistrationManager;
            _impersonationManager = impersonationManager;
            _userLinkManager = userLinkManager;
            _appNotifier = appNotifier;
            _smsSender = smsSender;
            _emailSender = emailSender;
            _externalAuthenticationSettingStore = externalAuthenticationSettingStore;
            _externalsettings = _externalAuthenticationSettingStore.GetSettingsAsync().Result;
            _verificationCodeManager = verificationCodeManager;
            _eventBus = eventBus;
            _identityOptions = identityOptions.Value;
            _tenantManager = tenantManager;
            _editionCache = editionCache;
        }

        #region Methods

        /// <summary>
        /// 认证登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AuthenticateResultModel> Authenticate([FromBody] AuthenticateModel model)
        {
            var loginResult = await GetLoginResultAsync(
                model.LoginCertificate,
                model.Password,
                null
            );

            if (model.SingleSignIn.HasValue && model.SingleSignIn.Value && loginResult.Result == AbpLoginResultType.Success)
            {
                loginResult.User.SetSignInToken();
            }

            //Password reset
            if (loginResult.User.ShouldChangePasswordOnNextLogin)
            {
                loginResult.User.SetNewPasswordResetCode();
                return new AuthenticateResultModel
                {
                    ShouldResetPassword = true,
                    PasswordResetCode = loginResult.User.PasswordResetCode,
                    UserId = loginResult.User.Id
                };
            }

            //Two factor auth
            await _userManager.InitializeOptionsAsync(loginResult.Tenant?.Id);
            string twoFactorRememberClientToken = null;
            if (await IsTwoFactorAuthRequiredAsync(loginResult, model))
            {
                if (model.TwoFactorVerificationCode.IsNullOrEmpty())
                {
                    //Add a cache item which will be checked in SendTwoFactorAuthCode to prevent sending unwanted two factor code to users.
                    _cacheManager
                        .GetTwoFactorCodeCache()
                        .Set(
                            loginResult.User.ToUserIdentifier().ToString(),
                            new TwoFactorCodeCacheItem()
                        );

                    return new AuthenticateResultModel
                    {
                        RequiresTwoFactorVerification = true,
                        UserId = loginResult.User.Id,
                        TwoFactorAuthProviders = await _userManager.GetValidTwoFactorProvidersAsync(loginResult.User)
                    };
                }

                twoFactorRememberClientToken = await TwoFactorAuthenticateAsync(loginResult.User, model);
            }

            //Login!
            var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
            return new AuthenticateResultModel
            {
                TenantId = loginResult.User.TenantId,
                AccessToken = accessToken,
                UserId = loginResult.User.Id,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
                TwoFactorRememberClientToken = twoFactorRememberClientToken
            };
        }

        /// <summary>
        /// 手机验证码认证登陆
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AuthenticateResultModel> PhoneNumAuthenticate([FromBody] PhoneAuthenticateModel model)
        {
            var loginResult = await GetLoginByPhoneCodeResultAsync(model.PhoneNum, model.LoginCode);

            //Password reset
            if (loginResult.User.ShouldChangePasswordOnNextLogin)
            {
                loginResult.User.SetNewPasswordResetCode();
                return new AuthenticateResultModel
                {
                    ShouldResetPassword = true,
                    PasswordResetCode = loginResult.User.PasswordResetCode,
                    UserId = loginResult.User.Id
                };
            }

            //Login!
            var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
            return new AuthenticateResultModel
            {
                TenantId = loginResult.User.TenantId,
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds,
            };
        }

        /// <summary>
        /// 注销登录
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize]
        public async Task LogOut()
        {
            if (AbpSession.UserId != null)
            {
                var tokenValidityKeyInClaims = User.Claims.First(c => c.Type == AppConsts.TokenValidityKey);
                await _userManager.RemoveTokenValidityKeyAsync(_userManager.GetUser(AbpSession.ToUserIdentifier()), tokenValidityKeyInClaims.Value);
                _cacheManager.GetCache(AppConsts.TokenValidityKey).Remove(tokenValidityKeyInClaims.Value);
            }
        }

        /// <summary>
        /// 发送双重认证码
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SendTwoFactorAuthCode([FromBody] SendTwoFactorAuthCodeModel model)
        {
            var cacheKey = new UserIdentifier(AbpSession.TenantId, model.UserId).ToString();

            var cacheItem = await _cacheManager
                .GetTwoFactorCodeCache()
                .GetOrDefaultAsync(cacheKey);

            if (cacheItem == null)
            {
                //There should be a cache item added in Authenticate method! This check is needed to prevent sending unwanted two factor code to users.
                throw new UserFriendlyException(L("SendSecurityCodeErrorMessage"));
            }

            var user = await _userManager.FindByIdAsync(model.UserId.ToString());

            cacheItem.Code = await _userManager.GenerateTwoFactorTokenAsync(user, model.Provider);

            var message = L("EmailSecurityCodeBody", cacheItem.Code);

            if (model.Provider == "Email")
            {
                await _emailSender.SendAsync(await _userManager.GetEmailAsync(user), L("EmailSecurityCodeSubject"), message);
            }
            else if (model.Provider == "Phone")
            {
                await _smsSender.SendAsync(new string[] { await _userManager.GetPhoneNumberAsync(user) }, message);
            }

            _cacheManager.GetTwoFactorCodeCache().Set(
                    cacheKey,
                    cacheItem
                );

            _cacheManager.GetCache("ProviderCache").Set(
                "Provider",
                model.Provider
            );
        }

        /// <summary>
        /// 模拟(用户)认证
        /// </summary>
        /// <param name="impersonationToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ImpersonatedAuthenticateResultModel> ImpersonatedAuthenticate(string impersonationToken)
        {
            var result = await _impersonationManager.GetImpersonatedUserAndIdentity(impersonationToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new ImpersonatedAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };
        }

        /// <summary>
        /// 关联用户认证
        /// </summary>
        /// <param name="switchAccountToken"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<SwitchedAccountAuthenticateResultModel> LinkedAccountAuthenticate(string switchAccountToken)
        {
            var result = await _userLinkManager.GetSwitchedUserAndIdentity(switchAccountToken);
            var accessToken = CreateAccessToken(await CreateJwtClaims(result.Identity, result.User));

            return new SwitchedAccountAuthenticateResultModel
            {
                AccessToken = accessToken,
                EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
            };
        }

        /// <summary>
        /// 获取第三方登陆供应商
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public List<ExternalLoginProviderInfoModel> GetExternalAuthenticationProviders()
        {
            return ObjectMapper.Map<List<ExternalLoginProviderInfoModel>>(_externalAuthConfiguration.Providers);
        }

        /// <summary>
        /// 第三方登陆认证
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<ExternalAuthenticateResultModel> ExternalAuthenticate([FromBody] ExternalAuthenticateModel model)
        {
            var externalUser = await GetExternalUserInfo(model);

            var loginResult = await _logInManager.LoginAsync(new ExternalUserLoginInfo(model.AuthProvider.GetUniversalProvider(), externalUser.UnionProviderKey, externalUser.ProviderKey, model.AuthProvider));

            switch (loginResult.Result)
            {
                //登陆成功
                case AbpLoginResultType.Success:
                    {
                        _eventBus.Trigger(new ExternalLoginEvent(loginResult.User, externalUser));

                        //更新授权信息
                        await _userManager.UpdateUserLoginAsync(externalUser);

                        var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));

                        if (model.SingleSignIn.HasValue && model.SingleSignIn.Value && loginResult.Result == AbpLoginResultType.Success)
                        {
                            loginResult.User.SetSignInToken();
                        }

                        return new ExternalAuthenticateResultModel
                        {
                            TenantId = loginResult.Tenant?.Id,
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                //未知的登陆(需要注册)
                case AbpLoginResultType.UnknownExternalLogin:
                    {
                        var newUser = await RegisterOrAssociateExternalUserAsync(externalUser); //第三方注册
                        if (!newUser.IsActive)
                        {
                            return new ExternalAuthenticateResultModel
                            {
                                UserId = newUser.Id,
                                WaitingForActivation = true,
                                NeedSupplementary = newUser.EmailAddress.IsNullOrEmpty() || newUser.UserName.IsNullOrEmpty(),
                            };
                        }

                        //Try to login again with newly registered user!
                        loginResult = await _logInManager.LoginAsync(new ExternalUserLoginInfo(model.AuthProvider.GetUniversalProvider(), externalUser.UnionProviderKey, externalUser.ProviderKey, model.AuthProvider));
                        if (loginResult.Result != AbpLoginResultType.Success)
                        {
                            throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                                loginResult.Result,
                                model.ProviderKey,
                                GetTenancyNameOrNull()
                            );
                        }

                        _eventBus.Trigger(new ExternalLoginEvent(newUser, externalUser));
                        var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));
                        return new ExternalAuthenticateResultModel
                        {
                            TenantId = loginResult.Tenant?.Id,
                            AccessToken = accessToken,
                            EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                            ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                        };
                    }
                //未激活的账号(需要补充资料)
                case AbpLoginResultType.UserIsNotActive:
                    {
                        var user = await _userManager.UserStore.FindAsync(AbpSession.TenantId, new UserLoginInfo(model.AuthProvider.GetUniversalProvider(), externalUser.ProviderKey, model.AuthProvider));
                        return new ExternalAuthenticateResultModel
                        {
                            UserId = user.Id,
                            WaitingForActivation = true,
                            NeedSupplementary = true,
                        };
                    }
                default:
                    {
                        throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(
                            loginResult.Result,
                            model.ProviderKey,
                            GetTenancyNameOrNull()
                        );
                    }
            }
        }

        /// <summary>
        /// 第三方账号绑定
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AbpAuthorize]
        public async Task ExternalBinding([FromBody] ExternalBindingModel model)
        {
            var externalUser = await GetExternalUserInfo(new ExternalAuthenticateModel()
            {
                AuthProvider = model.AuthProvider,
                ProviderAccessCode = model.ProviderAccessCode,
                ProviderKey = model.ProviderKey,
            });

            // 当前认证用户
            var userFound = await _userManager.UserStore.Find4PlatformAsync(new ExternalUserLoginInfo(model.AuthProvider.GetUniversalProvider(), externalUser.UnionProviderKey, externalUser.ProviderKey, model.AuthProvider));

            // 当前登录用户
            var userLoggedIn = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());

            if (userFound != null && userFound != userLoggedIn)
            {
                throw new UserFriendlyException(string.Format(L("ExternalBinging.AlreadyBingingUser"), L(model.AuthProvider)));
            }

            if (await _userManager.UserStore.IfUserAssociateExternal(userLoggedIn, model.AuthProvider.GetUniversalProvider()))
            {
                throw new UserFriendlyException(string.Format(L("ExternalBinging.AlreadyBingingExternal"), L(model.AuthProvider)));
            }

            userLoggedIn.Logins.Add(new ExternalUserLogin
            {
                UserName = externalUser.EmailAddress,
                ExternalDisplayName = externalUser.Name,
                LoginProvider = externalUser.Provider,
                UnionProviderKey = externalUser.UnionProviderKey,
                ProviderKey = externalUser.ProviderKey,
                TenantId = userLoggedIn.TenantId,
                AccessToken = externalUser.AccessToken,
                AccessTokenOutDataTime = externalUser.AccessTokenOutDataTime,
                RefreshToken = externalUser.RefreshToken,
            });

            await CurrentUnitOfWork.SaveChangesAsync();

            _eventBus.Trigger(new ExternalLoginEvent(userLoggedIn, externalUser));
        }

        /// <summary>
        /// 第三方账号解绑
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AbpAuthorize]
        public async Task ExternalUnBinding([FromBody] ExternalUnBindingModel model)
        {
            // 当前登录用户
            var userLoggedIn = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            await _userManager.UserStore.UserRepository.EnsureCollectionLoadedAsync(userLoggedIn, x => x.Logins);
            var externalBindingRecord = userLoggedIn.Logins.FirstOrDefault(el => el.LoginProvider == model.AuthProvider);

            if (externalBindingRecord == null)
                throw new UserFriendlyException(string.Format(L("ExternalBinging.UnBingingExternal"), L(model.AuthProvider)));

            userLoggedIn.Logins.Remove(externalBindingRecord);

            await CurrentUnitOfWork.SaveChangesAsync();
        }

        /// <summary>
        /// 补充注册(机构用户,需要登录)
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [AbpAuthorize]
        public async Task<SupplementAuthResultModel> SupplementAuth([FromBody] SupplementAuthModel model)
        {
            CheckSupplementAuthCondition(model);

            // 获取当前登录的用户
            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());

            if (user == null)
                throw new UserFriendlyException(L("CanNotFindUser"));

            // 已绑定租户的用户无法再次注册成为租户
            if (user.TenantId.HasValue)
                throw new UserFriendlyException(L("Identity.HadBindingTenant"));

            // 给当前用户添加租户并绑定
            var defaultEdition = await _editionCache.GetDefaultAsync();
            var tenantId = await _tenantManager.CreateWithExistUserAsync(
               userId: user.Id,
               tenancyName: model.TenantName,
               connectionString: null,
               isActive: true,
               editionId: defaultEdition.Id,
               sendActivationEmail: true,
               subscriptionEndDate: null,
               isInTrialPeriod: false,
               emailActivationLink: _appUrlService.CreateEmailActivationUrlFormat(model.TenantName));

            using (var uow = UnitOfWorkManager.Begin(TransactionScopeOption.RequiresNew))
            {
                using (UnitOfWorkManager.Current.SetTenantId(tenantId))
                {
                    var newUser = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
                    var tenant = await _tenantManager.FindByIdAsync(tenantId);

                    var loginResult = await _logInManager.ImmediateCreateLoginResultAsync(newUser, tenant);
                    var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));

                    //返回登陆是否需要激活邮箱
                    var isEmailConfirmationRequiredForLogin = await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.IsEmailConfirmationRequiredForLogin);

                    await uow.CompleteAsync();

                    return new SupplementAuthResultModel
                    {
                        CanLogin = newUser.IsActive && (newUser.IsEmailConfirmed || !isEmailConfirmationRequiredForLogin),
                        TenantId = loginResult.Tenant.Id,
                        AccessToken = accessToken,
                        EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                        ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                    };
                }

            }
        }

        /// <summary>
        /// 获取登录凭证短链
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [AbpAuthorize]
        public async Task<ShortAuthTokenModel> GetShortAuthToken()
        {
            // 获取当前登录的用户
            var user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
            var tenant = await _tenantManager.FindByIdAsync(AbpSession.GetTenantId());

            if (user == null)
                throw new UserFriendlyException(L("CanNotFindUser"));

            // 已绑定租户的用户无法再次注册成为租户
            if (!user.TenantId.HasValue)
                throw new UserFriendlyException(L("CanNotFindTenant"));

            var shortToken = Guid.NewGuid();
            var result = await _cacheManager.GetAuthenticateResultCache()
                .GetAsync(shortToken.ToString(), async () =>
                {
                    var loginResult = await _logInManager.ImmediateCreateLoginResultAsync(user, tenant);
                    var accessToken = CreateAccessToken(await CreateJwtClaims(loginResult.Identity, loginResult.User));

                    return new AuthenticateResultCacheItem
                    {
                        TenantId = loginResult.User.TenantId,
                        AccessToken = accessToken,
                        UserId = loginResult.User.Id,
                        EncryptedAccessToken = GetEncrpyedAccessToken(accessToken),
                        ExpireInSeconds = (int)_configuration.Expiration.TotalSeconds
                    };
                });

            return new ShortAuthTokenModel()
            {
                ShortAuthToken = shortToken.ToString()
            };
        }

        /// <summary>
        /// 使用短链凭证登陆
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<AuthenticateResultModel> AuthenticateByShortAuth([FromBody]ShortAuthTokenModel model)
        {
            var result = await _cacheManager.GetAuthenticateResultCache().GetOrDefaultAsync(model.ShortAuthToken);

            if (result == null)
                throw new UserFriendlyException(L("ShortAuthToken.OutOfDate"));

            await _cacheManager.GetAuthenticateResultCache().RemoveAsync(model.ShortAuthToken);
            return new AuthenticateResultModel
            {
                UserId = result.UserId,
                TenantId = result.TenantId,
                AccessToken = result.AccessToken,
                EncryptedAccessToken = result.EncryptedAccessToken,
                ExpireInSeconds = result.ExpireInSeconds,
            };
        }

        #endregion

        #region Etc

        /// <summary>
        /// 测试通知
        /// </summary>
        /// <param name="message">通知内容</param>
        /// <param name="severity">通知类型</param>
        /// <returns></returns>
        [AbpMvcAuthorize]
        [HttpGet]
        public async Task<ActionResult> TestNotification(string message = "", string severity = "info")
        {
            if (message.IsNullOrEmpty())
            {
                message = "This is a test notification, created at " + Clock.Now;
            }

            await _appNotifier.SendMessageAsync(
                AbpSession.ToUserIdentifier(),
                message,
                severity.ToPascalCase().ToEnum<NotificationSeverity>()
                );

            return Content("Sent notification: " + message);
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 第三方登陆注册
        /// </summary>
        /// <param name="externalUser"></param>
        /// <returns></returns>
        private async Task<User> RegisterOrAssociateExternalUserAsync(ExternalLoginUserInfo externalUser)
        {
            User user;

            // 如果当前已经登录，则绑定当前登陆用户
            // 否者创建新用户
            if (AbpSession.UserId.HasValue)
            {
                user = await _userManager.FindByIdAsync(AbpSession.GetUserId().ToString());
                await _userManager.UserStore.UserRepository.EnsureCollectionLoadedAsync(user, u => u.Logins);
            }
            else
            {
                user = await _userRegistrationManager.RegisterAsync(
                    externalUser.Name,
                    externalUser.EmailAddress,
                    externalUser.ProviderKey,
                    string.Empty,
                    Authorization.Users.User.CreateRandomPassword(),
                    false,
                    false);
                user.IsActive = CheckConditionForActive(user);
                user.Logins = new List<UserLogin>();
            }

            user.Logins.Add(new ExternalUserLogin
            {
                UserName = externalUser.UserName,
                ExternalDisplayName = externalUser.Name,
                LoginProvider = externalUser.Provider,
                UnionProviderKey = externalUser.UnionProviderKey,
                ProviderKey = externalUser.ProviderKey,
                TenantId = user.TenantId,
                AccessToken = externalUser.AccessToken,
                AccessTokenOutDataTime = externalUser.AccessTokenOutDataTime,
                RefreshToken = externalUser.RefreshToken,
            });

            await CurrentUnitOfWork.SaveChangesAsync();

            return user;
        }

        /// <summary>
        /// 获取第三方登陆信息
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private async Task<ExternalLoginUserInfo> GetExternalUserInfo(ExternalAuthenticateModel model)
        {
            var userInfo = await _externalAuthManager.GetUserInfo(model.AuthProvider, model.ProviderAccessCode);
            userInfo.Provider = userInfo.Provider.GetUniversalProvider();
            return userInfo;
        }

        /// <summary>
        /// 是否双重认证登陆请求
        /// </summary>
        /// <param name="loginResult"></param>
        /// <param name="authenticateModel"></param>
        /// <returns></returns>
        private async Task<bool> IsTwoFactorAuthRequiredAsync(AbpLoginResult<Tenant, User> loginResult, AuthenticateModel authenticateModel)
        {
            if (!await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsEnabled))
            {
                return false;
            }

            if (!loginResult.User.IsTwoFactorEnabled)
            {
                return false;
            }

            if ((await _userManager.GetValidTwoFactorProvidersAsync(loginResult.User)).Count <= 0)
            {
                return false;
            }

            if (await TwoFactorClientRememberedAsync(loginResult.User.ToUserIdentifier(), authenticateModel))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 双重验证客户端保存
        /// </summary>
        /// <param name="userIdentifier"></param>
        /// <param name="authenticateModel"></param>
        /// <returns></returns>
        private async Task<bool> TwoFactorClientRememberedAsync(UserIdentifier userIdentifier, AuthenticateModel authenticateModel)
        {
            if (!await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled))
            {
                return false;
            }

            if (string.IsNullOrWhiteSpace(authenticateModel.TwoFactorRememberClientToken))
            {
                return false;
            }

            try
            {
                var validationParameters = new TokenValidationParameters
                {
                    ValidAudience = _configuration.Audience,
                    ValidIssuer = _configuration.Issuer,
                    IssuerSigningKey = _configuration.SecurityKey
                };

                foreach (var validator in _jwtOptions.Value.SecurityTokenValidators)
                {
                    if (validator.CanReadToken(authenticateModel.TwoFactorRememberClientToken))
                    {
                        try
                        {
                            SecurityToken validatedToken;
                            var principal = validator.ValidateToken(authenticateModel.TwoFactorRememberClientToken, validationParameters, out validatedToken);
                            var useridentifierClaim = principal.FindFirst(c => c.Type == UserIdentifierClaimType);
                            if (useridentifierClaim == null)
                            {
                                return false;
                            }

                            return useridentifierClaim.Value == userIdentifier.ToString();
                        }
                        catch (Exception ex)
                        {
                            Logger.Debug(ex.ToString(), ex);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.Debug(ex.ToString(), ex);
            }

            return false;
        }

        /// <summary>
        /// 双重认证登陆
        /// </summary>
        /// <param name="user"></param>
        /// <param name="authenticateModel"></param>
        /// <returns></returns>
        /* Checkes two factor code and returns a token to remember the client (browser) if needed */
        private async Task<string> TwoFactorAuthenticateAsync(User user, AuthenticateModel authenticateModel)
        {
            var twoFactorCodeCache = _cacheManager.GetTwoFactorCodeCache();
            var userIdentifier = user.ToUserIdentifier().ToString();

            var cachedCode = await twoFactorCodeCache.GetOrDefaultAsync(userIdentifier);

            if (cachedCode?.Code == null || cachedCode.Code != authenticateModel.TwoFactorVerificationCode)
            {
                throw new UserFriendlyException(L("InvalidSecurityCode"));
            }

            //Delete from the cache since it was a single usage code
            await twoFactorCodeCache.RemoveAsync(userIdentifier);

            if (authenticateModel.RememberClient)
            {
                if (await SettingManager.GetSettingValueAsync<bool>(AbpZeroSettingNames.UserManagement.TwoFactorLogin.IsRememberBrowserEnabled))
                {
                    return CreateAccessToken(new[]
                        {
                            new Claim(UserIdentifierClaimType, user.ToUserIdentifier().ToString())
                        },
                        TimeSpan.FromDays(365)
                    );
                }
            }

            return null;
        }

        /// <summary>
        /// 获取租户名称
        /// </summary>
        /// <returns></returns>
        private string GetTenancyNameOrNull()
        {
            if (!AbpSession.TenantId.HasValue)
            {
                return null;
            }

            return _tenantCache.GetOrNull(AbpSession.TenantId.Value)?.TenancyName;
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="LoginCertificate"></param>
        /// <param name="password"></param>
        /// <param name="tenancyName"></param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginResultAsync(string LoginCertificate, string password, string tenancyName)
        {
            var loginResult = await _logInManager.LoginAsync(LoginCertificate, password, tenancyName);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, LoginCertificate, tenancyName);
            }
        }

        /// <summary>
        /// 登陆
        /// </summary>
        /// <param name="phoneNum"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        private async Task<AbpLoginResult<Tenant, User>> GetLoginByPhoneCodeResultAsync(string phoneNum, string code)
        {
            var loginResult = await _logInManager.LoginWithPhoneCodeAsync(phoneNum, code);

            switch (loginResult.Result)
            {
                case AbpLoginResultType.Success:
                    return loginResult;
                default:
                    throw _abpLoginResultTypeHelper.CreateExceptionForFailedLoginAttempt(loginResult.Result, phoneNum, string.Empty);
            }
        }

        /// <summary>
        /// 创建访问令牌
        /// </summary>
        /// <param name="claims"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        private string CreateAccessToken(IEnumerable<Claim> claims, TimeSpan? expiration = null)
        {
            var now = DateTime.UtcNow;

            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration.Issuer,
                audience: _configuration.Audience,
                claims: claims,
                notBefore: now,
                expires: now.Add(expiration ?? _configuration.Expiration),
                signingCredentials: _configuration.SigningCredentials
            );

            return new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);
        }

        /// <summary>
        /// 获取访问令牌编码
        /// </summary>
        /// <param name="accessToken"></param>
        /// <returns></returns>
        private string GetEncrpyedAccessToken(string accessToken)
        {
            return SimpleStringCipher.Instance.Encrypt(accessToken, AppConsts.DefaultPassPhrase);
        }

        /// <summary>
        /// 创建 Jwt 请求
        /// </summary>
        /// <param name="identity"></param>
        /// <param name="user"></param>
        /// <param name="expiration"></param>
        /// <returns></returns>
        private async Task<IEnumerable<Claim>> CreateJwtClaims(ClaimsIdentity identity, User user, TimeSpan? expiration = null)
        {
            var tokenValidityKey = Guid.NewGuid().ToString();
            var claims = identity.Claims.ToList();
            var nameIdClaim = claims.First(c => c.Type == _identityOptions.ClaimsIdentity.UserIdClaimType);

            if (_identityOptions.ClaimsIdentity.UserIdClaimType != JwtRegisteredClaimNames.Sub)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Sub, nameIdClaim.Value));
            }

            var userIdentifier = new UserIdentifier(AbpSession.TenantId, Convert.ToInt64(nameIdClaim.Value));

            claims.AddRange(new[]
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, DateTimeOffset.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
                new Claim(AppConsts.TokenValidityKey, tokenValidityKey),
                new Claim(AppConsts.UserIdentifier, userIdentifier.ToUserIdentifierString())
            });

            _cacheManager
                .GetCache(AppConsts.TokenValidityKey)
                .Set(tokenValidityKey, "");

            await _userManager.AddTokenValidityKeyAsync(user, tokenValidityKey,
                DateTime.UtcNow.Add(expiration ?? _configuration.Expiration));

            return claims;
        }

        private string AddSingleSignInParametersToReturnUrl(string returnUrl, string signInToken, long userId, int? tenantId)
        {
            returnUrl += (returnUrl.Contains("?") ? "&" : "?") +
                         "accessToken=" + signInToken +
                         "&userId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(userId.ToString()));
            if (tenantId.HasValue)
            {
                returnUrl += "&tenantId=" + Convert.ToBase64String(Encoding.UTF8.GetBytes(tenantId.Value.ToString()));
            }

            return returnUrl;
        }

        private bool UseCaptchaOnRegistration()
        {
            if (DebugHelper.IsDebug)
            {
                return false;
            }

            return SettingManager.GetSettingValue<bool>(AppSettings.UserManagement.UseCaptchaOnRegistration);
        }

        /// <summary>
        ///  验证是否满足激活条件
        /// </summary>
        /// <returns></returns>
        private bool CheckConditionForActive(User user)
        {
            if (_externalsettings.UserActivation == UserActivationOption.AutoActive)
                return true;

            if (_externalsettings.UserActivation == UserActivationOption.DonotActive)
                return false;

            if (_externalsettings.UserActivation == UserActivationOption.ActiveByCondition)
            {
                if (_externalsettings.RequiredUserName && user.UserName.IsNullOrEmpty())
                    return false;

                if (_externalsettings.RequiredEmail && user.EmailAddress.IsNullOrEmpty())
                    return false;

                if (_externalsettings.RequiredTelephone && user.PhoneNumber.IsNullOrEmpty())
                    return false;
            }

            return true;
        }

        /// <summary>
        /// 检查外部登陆，补充注册条件
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        private void CheckSupplementAuthCondition(SupplementAuthModel model)
        {
            if (model.TenantName.IsNullOrEmpty())
            {
                throw new UserFriendlyException(L("Identity.RequiredTenantName"));
            }
        }

        #endregion
    }
}
