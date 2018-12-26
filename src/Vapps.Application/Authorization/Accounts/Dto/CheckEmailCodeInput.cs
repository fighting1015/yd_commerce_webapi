using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Identity;
using Vapps.SMS.Dto;

namespace Vapps.Authorization.Accounts.Dto
{
    public class CheckEmailCodeInput : BaseSendInput
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
