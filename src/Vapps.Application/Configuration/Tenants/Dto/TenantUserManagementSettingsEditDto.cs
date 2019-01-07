namespace Vapps.Configuration.Tenants.Dto
{
    public class TenantUserManagementSettingsEditDto
    {
        /// <summary>
        /// 允许租户注册
        /// </summary>
        public bool AllowSelfRegistration { get; set; }

        /// <summary>
        /// 新注册用户默认激活
        /// </summary>
        public bool IsNewRegisteredUserActiveByDefault { get; set; }

        /// <summary>
        /// 登录前须邮箱确认
        /// </summary>
        public bool IsEmailConfirmationRequiredForLogin { get; set; }

        /// <summary>
        /// 开启(图形)验证码
        /// </summary>
        public bool UseCaptchaOnRegistration { get; set; }
    }
}