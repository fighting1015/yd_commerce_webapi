using System.ComponentModel.DataAnnotations;
using Vapps.Identity;

namespace Vapps.Authorization.Accounts.Dto
{
    public class SendEmailActivationLinkInput
    {
        /// <summary>
        /// 待激活账号邮箱地址
        /// </summary>
        [Required]
        public string EmailAddress { get; set; }
    }

    public class SendEmailVerificationCodeInput
    {
        /// <summary>
        /// 收件邮箱地址
        /// </summary>
        [Required]
        public string EmailAddress { get; set; }

        /// <summary>
        /// 验证类型
        /// </summary>
        [EnumDataType(typeof(VerificationCodeType))]
        public VerificationCodeType CodeType { get; set; }
    }
}