namespace Vapps.Identity
{
    /// <summary>
    /// 验证码类型
    /// </summary>
    public enum VerificationCodeType
    {
        /// <summary>
        /// 注册
        /// </summary>
        Register = 10,

        /// <summary>
        /// 登陆
        /// </summary>
        Login = 20,

        /// <summary>
        /// 修改密码
        /// </summary>
        ChangePassword = 30,

        /// <summary>
        /// 邮箱绑定
        /// </summary>
        EmailBinding = 40,

        /// <summary>
        /// 解绑邮箱
        /// </summary>
        EmailUnBinding = 50,

        /// <summary>
        /// 手机绑定
        /// </summary>
        PhoneBinding = 60,

        /// <summary>
        /// 手机解绑
        /// </summary>
        PhoneUnBinding = 70,

        /// <summary>
        /// 手机验证
        /// </summary>
        PhoneVerify = 80,
    }
}
