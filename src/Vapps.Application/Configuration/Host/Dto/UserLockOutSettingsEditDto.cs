namespace Vapps.Configuration.Host.Dto
{
    /// <summary>
    /// 账户锁定设置
    /// </summary>
    public class UserLockOutSettingsEditDto
    {
        /// <summary>
        /// 启用
        /// </summary>
        public bool IsEnabled { get; set; }

        /// <summary>
        /// 最大访问失败(密码错误)次数
        /// </summary>
        public int MaxFailedAccessAttemptsBeforeLockout { get; set; }

        /// <summary>
        /// 默认锁定时间（单位：秒）
        /// </summary>
        public int DefaultAccountLockoutSeconds { get; set; }
    }
}