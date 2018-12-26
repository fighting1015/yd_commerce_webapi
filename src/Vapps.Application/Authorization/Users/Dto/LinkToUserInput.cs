using Abp.Auditing;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Users.Dto
{
    public class LinkToUserInput
    {
        /// <summary>
        /// 租户名称
        /// </summary>
        public string TenancyName { get; set; }

        /// <summary>
        /// 用户名或邮箱地址
        /// </summary>
        [Required]
        public string UsernameOrEmailAddress { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [Required]
        [DisableAuditing]
        public string Password { get; set; }
    }
}