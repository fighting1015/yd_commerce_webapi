using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace Vapps.Authorization.Accounts.Dto
{
    public class SendPasswordResetCodeInput
    {
        /// <summary>
        /// 需要重置密码的账号邮箱
        /// </summary>
        [Required]
        [MaxLength(AbpUserBase.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}