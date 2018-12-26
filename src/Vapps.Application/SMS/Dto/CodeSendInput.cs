using System.ComponentModel.DataAnnotations;
using Vapps.Attributes;
using Vapps.Identity;

namespace Vapps.SMS.Dto
{
    public class CodeSendInput : BaseSendInput
    {
        /// <summary>
        /// 目标号码(数组)
        /// </summary>
        [PhoneNumber(false)]
        public string TargetNumber { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        [EnumDataType(typeof(VerificationCodeType))]
        public VerificationCodeType CodeType { get; set; }
    }

    public class UserCodeSendInput : BaseSendInput
    {
        /// <summary>
        /// 验证码类型
        /// </summary>
        [EnumDataType(typeof(VerificationCodeType))]
        public VerificationCodeType CodeType { get; set; }
    }

    public class CheckUserCodeInput : BaseSendInput
    {
        /// <summary>
        ///  验证码
        /// </summary>
        [Required]
        [StringLength(6)]
        public string Code { get; set; }

        /// <summary>
        /// 验证码类型
        /// </summary>
        [EnumDataType(typeof(VerificationCodeType))]
        public VerificationCodeType CodeType { get; set; }
    }
}
