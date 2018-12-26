using Abp.Configuration;
using Abp.Dependency;
using Abp.Localization;
using Abp.MultiTenancy;
using Abp.Timing;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using Vapps.Authorization.Users;
using Vapps.Configuration;
using Vapps.Extensions;
using Vapps.Helpers;
using Vapps.Identity.Cache;
using Vapps.MultiTenancy;

namespace Vapps.Messages
{
    public class MessageTokenProvider : IMessageTokenProvider, ITransientDependency
    {
        private const string UserTokenName = "UserToken";
        private const string VerificationCodeTokenName = "VerificationCodeToken";

        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;
        private readonly ILocalizationManager _localizationManager;
        private readonly ITenantCache _tenantCache;
        private readonly ITokenizer _tokenizer;
        private readonly ISettingManager _settingManager;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly TenantManager _tenantManager;

        public MessageTokenProvider(ITenantCache tenantCache,
            IHostingEnvironment env,
            ITokenizer tokenizer,
            ISettingManager settingManager,
            ILocalizationManager localizationManager,
            IDateTimeHelper dateTimeHelper,
            TenantManager tenantManager)
        {
            this._env = env;
            this._tenantCache = tenantCache;
            this._tokenizer = tokenizer;
            this._settingManager = settingManager;
            this._tenantManager = tenantManager;
            this._appConfiguration = _env.GetAppConfiguration();
            this._localizationManager = localizationManager;
            this._dateTimeHelper = dateTimeHelper;
        }

        #region Methods

        /// <summary>
        /// 替换指令
        /// </summary>
        /// <param name="templateWithToken"></param>
        /// <param name="user"></param>
        /// <param name="verificationCode"></param>
        /// <returns></returns>
        public virtual string ReplaceToken(string templateWithToken,
            User user = null,
            VerificationCodeCacheItem verificationCode = null)
        {
            var tokens = new List<Token>();

            if (user != null)
                AddUserTokens(tokens, user, string.Empty);

            if (verificationCode != null)
                AddVerificationCodeTokens(tokens, verificationCode);

            string result = _tokenizer.Replace(templateWithToken, tokens, false);
            return result;
        }

        /// <summary>
        /// 获取所有可用指令
        /// </summary>
        /// <returns></returns>
        public Dictionary<string, string> GetAllowedTokens()
        {
            var result = new Dictionary<string, string>();
            result.Add(UserTokenName, string.Join(",", GetListOfUserAllowedTokens()));
            result.Add(VerificationCodeTokenName, string.Join(",", GetListOfVerificationCodeAllowedTokens()));

            return result;
        }

        /// <summary>
        /// 用户指令 TODO:待补充
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="user"></param>
        /// <param name="authProviderName"></param>
        public virtual void AddUserTokens(IList<Token> tokens, User user, string authProviderName = "")
        {
            if (user.TenantId.HasValue)
            {
                var tenant = _tenantManager.GetById(user.TenantId.Value);
                tokens.Add(new Token("User.Tenant", tenant.Name));
            }

            tokens.Add(new Token("User.Id", user.Id.ToString()));
            tokens.Add(new Token("User.Email", user.EmailAddress));
            tokens.Add(new Token("User.EmailConfirmationCode", user.EmailConfirmationCode));
            tokens.Add(new Token("User.Username", user.UserName));
            tokens.Add(new Token("User.Tenant", user.UserName));
            tokens.Add(new Token("User.PhoneNumber", user.UserName));
            tokens.Add(new Token("User.Name", user.Name));
            tokens.Add(new Token("User.Surname", user.Surname));
            tokens.Add(new Token("User.RegisterOn", ClockProviders.Local.Normalize(user.CreationTime).ToString("yyyy-MM-dd HH:mm")));
        }

        /// <summary>
        /// 验证码指令
        /// </summary>
        /// <param name="tokens"></param>
        /// <param name="verificationCode"></param>
        public virtual void AddVerificationCodeTokens(IList<Token> tokens, VerificationCodeCacheItem verificationCode)
        {
            var availableSecond = _settingManager.GetSettingValue<int>(AppSettings.UserManagement.VerificationCodeManagement.AvailableSecond);
            tokens.Add(new Token("VerificationCode.Code", verificationCode.Code.ToString()));
            tokens.Add(new Token("VerificationCode.AvailableSecond", availableSecond.ToString()));
            tokens.Add(new Token("VerificationCode.AvailableMinute", (availableSecond / 60).ToString()));
        }

        /// <summary>
        /// 获取所有用户指令
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetListOfUserAllowedTokens()
        {
            var allowedTokens = new List<string>()
            {
                "%User.Id%",
                "%User.Tenant%",
                "%User.Email%",
                "%User.EmailConfirmationCode%",
                "%User.Username%",
                "%User.PhoneNumber%",
                "%User.Name%",
                "%User.Surname%",
                "%User.RegisterOn%",
            };
            return allowedTokens.ToArray();
        }

        /// <summary>
        /// 获取所有
        /// </summary>
        /// <returns></returns>
        public virtual string[] GetListOfVerificationCodeAllowedTokens()
        {
            var allowedTokens = new List<string>()
            {
                "%VerificationCode.Code%",
                "%VerificationCode.AvailableSecond%",
                "%VerificationCode.AvailableMinute%",
            };
            return allowedTokens.ToArray();
        }

        #endregion

        #region Utilities

        private string GetUserCenterDomainUrl()
        {
            return _appConfiguration["App:UserCenterAddress"];

        }

        private string GetBusinessDomainUrl()
        {
            return _appConfiguration["App:BusinessCenterAddress"];

        }

        #endregion
    }
}
