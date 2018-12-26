using System.Collections.Generic;
using Vapps.Common.Dto;

namespace Vapps.Configuration.Host.Dto
{
    public class SMSSettingsEditDto
    {
        /// <summary>
        /// 是否启用图形验证码
        /// </summary>
        public bool UseCaptchaToVerification { get; set; }

        /// <summary>
        /// 注册模板
        /// </summary>
        public int RegisterVerificationTempId { get; set; }

        /// <summary>
        /// 修改密码模板
        /// </summary>
        public int ChangePasswordVerificationTempId { get; set; }

        /// <summary>
        /// 绑定手机模板
        /// </summary>
        public int UnBindingPhoneVerificationTempId { get; set; }

        /// <summary>
        /// 绑定手机模板
        /// </summary>
        public int BindingPhoneVerificationTempId { get; set; }

        /// <summary>
        /// 登陆模板
        /// </summary>
        public int LoginVerificationTempId { get; set; }

        /// <summary>
        /// 手机验证模板
        /// </summary>
        public int PhoneVerificationTempId { get; set; }

        /// <summary>
        /// 可用短信模板
        /// </summary>
        public List<SelectListItemDto> AvailableSmsTemplates { get; set; }
    }
}
