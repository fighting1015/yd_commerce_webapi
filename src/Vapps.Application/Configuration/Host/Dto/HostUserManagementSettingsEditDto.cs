namespace Vapps.Configuration.Host.Dto
{
    public class HostUserManagementSettingsEditDto
    {
        /// <summary>
        /// 必须验证邮箱地址后才能登录
        /// </summary>
        public bool IsEmailConfirmationRequiredForLogin { get; set; }
    }
}