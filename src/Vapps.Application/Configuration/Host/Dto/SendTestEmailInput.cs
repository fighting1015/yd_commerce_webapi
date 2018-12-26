using System.ComponentModel.DataAnnotations;
using Vapps.Authorization.Users;

namespace Vapps.Configuration.Host.Dto
{
    public class SendTestEmailInput
    {
        /// <summary>
        /// 目标邮箱地址
        /// </summary>
        [Required]
        [MaxLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }
    }
}