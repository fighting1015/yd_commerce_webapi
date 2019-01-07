using Abp.Configuration;
using Abp.Domain.Uow;
using Abp.MultiTenancy;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Abp.UI;
using System;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.Helpers;
using Vapps.Identity.Cache;
using Vapps.Runtime.Caching;

namespace Vapps.Identity
{
    public class VerificationCodeManager : VappsDomainServiceBase, IVerificationCodeManager
    {
        private readonly ICacheManager _cacheManager;
        private readonly ITenantCache _tenantCache;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAbpSession _abpSession;

        private readonly object _syncObj = new object();

        public VerificationCodeManager(ICacheManager cacheManager,
            ITenantCache tenantCache,
            IUnitOfWorkManager unitOfWorkManager,
            IAbpSession abpSession)
        {
            this._cacheManager = cacheManager;
            this._tenantCache = tenantCache;
            this._unitOfWorkManager = unitOfWorkManager;
            this._abpSession = abpSession;
        }

        /// <summary>
        /// 保存验证码信息
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <param name="customer"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<VerificationCodeCacheItem> GetOrSetVerificationCodeAsync(string phoneOrEmail, VerificationCodeType type)
        {
            var minimumSendInterval = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.VerificationCodeManagement.MinimumSendInterval);
            var availableSecond = await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.VerificationCodeManagement.AvailableSecond);

            string cacheKey = GetCacheKey(phoneOrEmail, type);
            var cacheItem = await GetVerificationCodeCacheItemOrNull(phoneOrEmail, type);
            if (cacheItem != null && cacheItem.ResendOnUtc > DateTime.UtcNow)
            {
                throw new UserFriendlyException(string.Format(L("GetVerificationCode.LessIntervalTime"),
                   await SettingManager.GetSettingValueAsync<int>(AppSettings.UserManagement.VerificationCodeManagement.MinimumSendInterval) / 60));
            }

            //重新保存验证码
            cacheItem = GenerateVerificationCode(phoneOrEmail, minimumSendInterval, availableSecond);
            await _cacheManager.GetVerificationCodeCache().SetAsync(cacheKey, cacheItem, TimeSpan.FromMinutes(availableSecond));

            return cacheItem;
        }

        /// <summary>
        /// 检查注册验证码
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        public virtual async Task CheckRegistrationVerificationCode(string phoneOrEmail, string code)
        {
            if (!await CheckVerificationCodeAsync(code, phoneOrEmail, VerificationCodeType.Register))
            {
                throw new UserFriendlyException(L("InvaildVerificationCode"));
            }
        }

        /// <summary>
        /// 检查验证码是否正确
        /// 如果正确，则消码并返回true
        /// 否者返回false
        /// </summary>
        /// <param name="code"></param>
        /// <param name="phoneOrEmail">手机号码</param>
        /// <param name="type">验证码</param>
        /// <param name="ignoreExpire">验证成功后是否忽略修改过期时间</param>
        /// <returns></returns>
        public virtual async Task<bool> CheckVerificationCodeAsync(string code,
            string phoneOrEmail,
            VerificationCodeType type,
            bool ignoreExpire = false)
        {
            var verificationCode = await GetVerificationCodeCacheItemOrNull(phoneOrEmail, type);
            if (verificationCode == null)
                return false;

            //验证码是否过期
            if (!ignoreExpire && verificationCode.ExpirationOnUtc < DateTime.UtcNow)
                return false;

            //验证的手机号码是否不一致
            if (verificationCode.PhoneOrEmail != phoneOrEmail)
                return false;

            if (verificationCode.Code.Equals(code))
            {
                if (!ignoreExpire)
                {
                    //验证通过，过期时间修改为现在
                    SetVerificationCodeExpiration(verificationCode);
                }
                return true;
            }
            else
                return false;
        }

        /// <summary>
        /// 获取验证码信息
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <param name="customer"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private async Task<VerificationCodeCacheItem> GetVerificationCodeCacheItemOrNull(string phoneOrEmail, VerificationCodeType type)
        {
            string cacheKey = GetCacheKey(phoneOrEmail, type);

            return await _cacheManager.GetVerificationCodeCache().GetOrDefaultAsync(cacheKey);
        }

        /// <summary>
        /// 设置验证码过期
        /// </summary>
        /// <param name="cacheItem"></param>
        protected virtual void SetVerificationCodeExpiration(VerificationCodeCacheItem cacheItem)
        {
            lock (_syncObj)
            {
                cacheItem.ExpirationOnUtc = DateTime.UtcNow;
            }
        }

        /// <summary>
        /// 缓存Key 
        /// {租户Id}-{手机号码/邮箱}-{验证码类型}
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <param name="type"></param>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        private string GetCacheKey(string phoneOrEmail, VerificationCodeType type)
        {
            //return $"{GetCurrentTenantId()}-{phoneOrEmail}-{type.ToString()}";
            return $"{phoneOrEmail}-{type.ToString()}";
        }

        /// <summary>
        /// 生成6位随机数字验证码
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <param name="minimumSendInterval"></param>
        /// <param name="availableSecond"></param>
        /// <returns></returns>
        public virtual VerificationCodeCacheItem GenerateVerificationCode(string phoneOrEmail, int minimumSendInterval, int availableSecond)
        {
            var verificationCode = new VerificationCodeCacheItem()
            {
                //Code = CommonHelper.GenerateRandomDigitCode(6),
                Code = "0000",
                ExpirationOnUtc = DateTime.UtcNow.AddSeconds(availableSecond),
                ResendOnUtc = DateTime.UtcNow.AddSeconds(minimumSendInterval),
                PhoneOrEmail = phoneOrEmail,
            };

            return verificationCode;
        }

        private int? GetCurrentTenantId()
        {
            if (_unitOfWorkManager.Current != null)
            {
                return _unitOfWorkManager.Current.GetTenantId();
            }

            return _abpSession.TenantId;
        }
    }
}
