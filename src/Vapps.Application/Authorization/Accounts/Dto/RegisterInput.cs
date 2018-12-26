using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Abp.Auditing;
using Abp.Authorization.Users;
using Abp.Extensions;
using Vapps.Authorization.Users;
using Vapps.Validation;
using Vapps.Attributes;
using Vapps.Identity;
using System;

namespace Vapps.Authorization.Accounts.Dto
{
    public class RegisterInput
    {
        /// <summary>
        /// 名称
        /// </summary>
        //[Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [PhoneNumber(true)]
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 邮箱
        /// </summary>
        [Attributes.EmailAddress(true)]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 注册类型
        /// </summary>
        public RegisterType Type { get; set; }

        /// <summary>
        /// 注册验证码
        /// </summary>
        [Required]
        public string RegisterCode { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [StringLength(User.MaxPlainPasswordLength)]
        [DisableAuditing]
        public string Password { get; set; }
    }
}