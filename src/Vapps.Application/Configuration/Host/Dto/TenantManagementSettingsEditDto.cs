namespace Vapps.Configuration.Host.Dto
{
    /// <summary>
    /// 租户管理设置
    /// </summary>
    public class TenantManagementSettingsEditDto
    {
        /// <summary>
        /// 允许租户注册
        /// </summary>
        public bool AllowSelfRegistration { get; set; }

        /// <summary>
        /// 新注册租户默认激活
        /// </summary>
        public bool IsNewRegisteredTenantActiveByDefault { get; set; }

        /// <summary>
        /// 开启(图形)验证码
        /// </summary>
        public bool UseCaptchaOnRegistration { get; set; }

        /// <summary>
        /// 默认版本Id
        /// </summary>
        public int? DefaultEditionId { get; set; }
    }
}