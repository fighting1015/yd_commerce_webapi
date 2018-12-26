using System.Threading.Tasks;
using Vapps.Identity.Cache;

namespace Vapps.Identity
{
    public interface IVerificationCodeManager
    {
        /// <summary>
        /// 获取验证码信息
        /// 如果没有则生成验证码
        /// 否者直接返回缓存中的验证码
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<VerificationCodeCacheItem> GetOrSetVerificationCodeAsync(string phoneOrEmail, VerificationCodeType type);

        /// <summary>
        /// 检查注册验证码
        /// </summary>
        /// <param name="phoneOrEmail"></param>
        /// <param name="code"></param>
        /// <returns></returns>
        Task CheckRegistrationVerificationCode(string phoneOrEmail, string code);

        /// <summary>
        /// 检查验证码是否正确
        /// 如果正确，则消码并返回true
        /// 否者返回false
        /// </summary>
        /// <param name="code"></param>
        /// <param name="phoneOrEmail"></param>
        /// <param name="customer"></param>
        /// <param name="type"></param>
        /// <param name="ignoreExpire"></param>
        /// <param name="storeId"></param>
        /// <returns></returns>
        Task<bool> CheckVerificationCodeAsync(string code,
            string phoneOrEmail,
            VerificationCodeType type,
            bool ignoreExpire = false);
    }
}
