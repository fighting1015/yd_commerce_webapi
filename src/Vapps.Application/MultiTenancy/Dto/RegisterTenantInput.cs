using Abp.Auditing;
using Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;
using Vapps.Attributes;
using Vapps.Identity;

namespace Vapps.MultiTenancy.Dto
{
    public class RegisterTenantInput
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        [Required]
        [StringLength(Tenant.NewMaxTenancyNameLength)]
        public string TenancyName { get; set; }

        /// <summary>
        /// 注册类型
        /// </summary>
        public RegisterType Type { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [PhoneNumber(true)]
        public string PhoneNumber { get; set; }

        [Attributes.EmailAddress(true)]
        [StringLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 注册验证码
        /// </summary>
        [Required]
        public string RegisterCode { get; set; }

        /// <summary>
        /// 管理员密码
        /// </summary>
        [StringLength(AbpUserBase.MaxPlainPasswordLength)]
        public string Password { get; set; }

        /// <summary>
        /// 验证码结果
        /// </summary>
        [DisableAuditing]
        public string CaptchaResponse { get; set; }
    }
}