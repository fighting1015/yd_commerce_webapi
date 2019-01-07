namespace Vapps.Security
{
    /// <summary>
    /// 密码复杂性设置
    /// </summary>
    public class PasswordComplexitySetting
    {
        public bool Equals(PasswordComplexitySetting other)
        {
            if (other == null)
            {
                return false;
            }

            return
                RequireDigit == other.RequireDigit &&
                RequireLowercase == other.RequireLowercase &&
                RequireNonAlphanumeric == other.RequireNonAlphanumeric &&
                RequireUppercase == other.RequireUppercase &&
                RequiredLength == other.RequiredLength;
        }

        /// <summary>
        /// 必须包含数字
        /// </summary>
        public bool RequireDigit { get; set; }

        /// <summary>
        /// 必须包含小写字母
        /// </summary>
        public bool RequireLowercase { get; set; }

        /// <summary>
        /// 必须包含大写
        /// </summary>
        public bool RequireUppercase { get; set; }

        /// <summary>
        /// 必须包含非字母数字(符号)
        /// </summary>
        public bool RequireNonAlphanumeric { get; set; }

        /// <summary>
        /// 最小长度限制
        /// </summary>
        public int RequiredLength { get; set; }
    }
}
