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
        /// 新注册
        /// </summary>
        public bool IsNewRegisteredTenantActiveByDefault { get; set; }

        public bool UseCaptchaOnRegistration { get; set; }

        public int? DefaultEditionId { get; set; }

    }
}