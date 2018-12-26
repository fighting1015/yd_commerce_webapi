using System.ComponentModel.DataAnnotations;

namespace Vapps.Web.Models.TokenAuth
{
    public class SendTwoFactorAuthCodeModel
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        /// <summary>
        /// 供应商（email/sms）
        /// </summary>
        [Required]
        public string Provider { get; set; }
    }
}