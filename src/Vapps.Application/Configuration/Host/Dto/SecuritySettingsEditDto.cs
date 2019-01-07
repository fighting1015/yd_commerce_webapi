using Vapps.Security;

namespace Vapps.Configuration.Host.Dto
{
    public class SecuritySettingsEditDto
    {
        /// <summary>
        /// 使用默认设置
        /// </summary>
        public bool UseDefaultPasswordComplexitySettings { get; set; }

        /// <summary>
        /// 密码复杂性
        /// </summary>
        public PasswordComplexitySetting PasswordComplexity { get; set; }

        /// <summary>
        /// 默认密码复杂性
        /// </summary>
        public PasswordComplexitySetting DefaultPasswordComplexity { get; set; }

        /// <summary>
        /// 用户锁定
        /// </summary>
        public UserLockOutSettingsEditDto UserLockOut { get; set; }

        /// <summary>
        /// 验证码设置
        /// </summary>
        public VerificationCodeSettingsEditDto VerificationCode { get; set; }
    }
}