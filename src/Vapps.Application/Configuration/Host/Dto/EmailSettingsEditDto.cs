namespace Vapps.Configuration.Host.Dto
{
    public class EmailSettingsEditDto
    {
        //No validation is done, since we may don't want to use email system.

        /// <summary>
        /// 默认发送邮箱
        /// </summary>
        public string DefaultFromAddress { get; set; }

        /// <summary>
        /// 默认发件人名字
        /// </summary>
        public string DefaultFromDisplayName { get; set; }

        /// <summary>
        /// SMTP 服务器地址
        /// </summary>
        public string SmtpHost { get; set; }

        /// <summary>
        /// SMTP 端口
        /// </summary>
        public int SmtpPort { get; set; }

        /// <summary>
        /// SMTP 用户名
        /// </summary>
        public string SmtpUserName { get; set; }

        /// <summary>
        /// SMTP 密码
        /// </summary>
        public string SmtpPassword { get; set; }

        /// <summary>
        /// SMTP 域名
        /// </summary>
        public string SmtpDomain { get; set; }

        /// <summary>
        /// 使用SSL
        /// </summary>
        public bool SmtpEnableSsl { get; set; }

        /// <summary>
        /// 默认身份验证
        /// </summary>
        public bool SmtpUseDefaultCredentials { get; set; }
    }
}