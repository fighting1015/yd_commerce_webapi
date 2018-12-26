using Abp.Auditing;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Users.Profile.Dto
{
    public class ChangePasswordByPhoneInput
    {
        /// <summary>
        /// 手机验证码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string Code { get; set; }

        /// <summary>
        /// 新密码
        /// </summary>
        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}
