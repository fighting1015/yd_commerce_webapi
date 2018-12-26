using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Vapps.ExternalAuthentications;

namespace Vapps.Configuration.Host.Dto
{
    public class HostSettingsEditDto
    {
        /// <summary>
        /// 基本信息
        /// </summary>
        [Required]
        public GeneralSettingsEditDto General { get; set; }

        /// <summary>
        /// 用户管理
        /// </summary>
        [Required]
        public HostUserManagementSettingsEditDto UserManagement { get; set; }

        /// <summary>
        /// 邮件
        /// </summary>
        [Required]
        public EmailSettingsEditDto Email { get; set; }

        /// <summary>
        /// 租户设置
        /// </summary>
        [Required]
        public TenantManagementSettingsEditDto TenantManagement { get; set; }

        /// <summary>
        /// 安全
        /// </summary>
        [Required]
        public SecuritySettingsEditDto Security { get; set; }

        /// <summary>
        /// 第三方登陆
        /// </summary>
        [Required]
        public ExternalAuthenticationEditDto ExternalAuthentication { get; set; }

        /// <summary>
        /// 短信设置
        /// </summary>
        [Required]
        public SMSSettingsEditDto SMSSettings { get; set; }

        /// <summary>
        /// 账单信息
        /// </summary>
        public HostBillingSettingsEditDto Billing { get; set; }
    }
}