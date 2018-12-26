using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Attributes;

namespace Vapps.Authorization.Users.Profile.Dto
{
    public class BindingEmailInput
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [Attributes.EmailAddress(false)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 邮箱验证码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string Code { get; set; }
    }

    public class ChangeBindingEmailInput
    {
        /// <summary>
        /// 解绑码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string ValidCode { get; set; }

        /// <summary>
        /// 新邮箱验证码
        /// </summary>
        [Attributes.EmailAddress(false)]
        public string NewEmailAddress { get; set; }

        /// <summary>
        /// 绑定码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string BindlingCode { get; set; }
    }
}
