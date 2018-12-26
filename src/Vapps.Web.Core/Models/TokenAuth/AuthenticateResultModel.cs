using System.Collections.Generic;

namespace Vapps.Web.Models.TokenAuth
{
    public class AuthenticateResultModel
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 访问令牌(加密)
        /// </summary>
        public string EncryptedAccessToken { get; set; }

        /// <summary>
        /// 过期时间
        /// </summary>
        public int ExpireInSeconds { get; set; }

        /// <summary>
        ///  需要重置密码
        /// </summary>
        public bool ShouldResetPassword { get; set; }

        /// <summary>
        /// 密码重置码
        /// </summary>
        public string PasswordResetCode { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 需要双重验证
        /// </summary>
        public bool RequiresTwoFactorVerification { get; set; }

        /// <summary>
        /// 双重认证供应商
        /// </summary>
        public IList<string> TwoFactorAuthProviders { get; set; }

        /// <summary>
        /// 记住双重认证 Token
        /// </summary>
        public string TwoFactorRememberClientToken { get; set; }
    }
}