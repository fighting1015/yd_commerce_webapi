using System;

namespace Vapps.Identity.Cache
{
    /// <summary>
    /// 验证码缓存类
    /// </summary>
    public class VerificationCodeCacheItem
    {
        public const string CacheName = "VerificationCodeCache";

        /// <summary>
        /// 验收手机号码/邮箱地址
        /// </summary>
        public string PhoneOrEmail { get; set; }

        /// <summary>
        /// 验证代码
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 验证码过期时间(有效)
        /// </summary>
        public DateTime ExpirationOnUtc { get; set; }

        /// <summary>
        /// 验证码可重新发送时间（利用此字段避免恶意注册）
        /// </summary>
        public DateTime ResendOnUtc { get; set; }
    }
}
