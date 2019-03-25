using System.ComponentModel.DataAnnotations;
using Vapps.Configuration.Host.Dto;

namespace Vapps.Install.Dto
{
    public class InstallDto
    {
        /// <summary>
        /// 数据库连接字符串
        /// </summary>
        [Required]
        public string ConnectionString { get; set; }

        /// <summary>
        /// 管理员密码(Admin)
        /// </summary>
        [Required]
        public string AdminPassword { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        [Required]
        public string WebSiteUrl { get; set; }

        /// <summary>
        /// 服务端(Api) Url
        /// </summary>
        public string ServerUrl { get; set; }

        /// <summary>
        /// 默认语言
        /// </summary>
        [Required]
        public string DefaultLanguage { get; set; }

        /// <summary>
        /// 邮箱设置
        /// </summary>
        public EmailSettingsEditDto SmtpSettings { get; set; }
    }
}