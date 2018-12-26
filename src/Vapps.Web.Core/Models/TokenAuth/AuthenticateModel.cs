using Abp.Auditing;
using Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;
using Vapps.Attributes;

namespace Vapps.Web.Models.TokenAuth
{
    public class AuthenticateModel
    {
        /// <summary>
        /// 登陆凭证(用户名/邮箱地址/手机)
        /// </summary>
        [Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string LoginCertificate { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [MaxLength(AbpUserBase.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }

        /// <summary>
        /// 双重认证码
        /// </summary>
        public string TwoFactorVerificationCode { get; set; }

        /// <summary>
        /// 在客户端记住
        /// </summary>
        public bool RememberClient { get; set; }

        /// <summary>
        /// 双重认证在客户端记住
        /// </summary>
        public string TwoFactorRememberClientToken { get; set; }

        /// <summary>
        /// 单步登陆
        /// </summary>
        public bool? SingleSignIn { get; set; }
    }

    public class PhoneAuthenticateModel
    {
        /// <summary>
        /// 手机
        /// </summary>
        [PhoneNumber(false)]
        public string PhoneNum { get; set; }

        /// <summary>
        /// 登陆验证码
        /// </summary>
        [Required]
        public string LoginCode { get; set; }

        /// <summary>
        /// 在客户端记住
        /// </summary>
        public bool RememberClient { get; set; }
    }
}