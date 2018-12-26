using System.ComponentModel.DataAnnotations;
using Vapps.Attributes;

namespace Vapps.Authorization.Users.Profile.Dto
{
    public class BindingPhoneNumInput
    {
        /// <summary>
        /// 手机号码
        /// </summary>
        [PhoneNumber]
        public string PhoneNum { get; set; }

        /// <summary>
        /// 手机验证码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string Code { get; set; }
    }
}
