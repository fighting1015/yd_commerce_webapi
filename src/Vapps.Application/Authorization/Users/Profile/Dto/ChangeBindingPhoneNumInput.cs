using System.ComponentModel.DataAnnotations;
using Vapps.Attributes;

namespace Vapps.Authorization.Users.Profile.Dto
{

    public class ChangeBindingPhoneNumInput
    {
        /// <summary>
        /// 解绑码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string ValidCode { get; set; }

        /// <summary>
        /// 手机号码
        /// </summary>
        [PhoneNumber]
        public string NewTelephone { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string BundlingCode { get; set; }
    }
}
