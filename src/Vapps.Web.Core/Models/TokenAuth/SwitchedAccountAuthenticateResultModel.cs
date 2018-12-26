namespace Vapps.Web.Models.TokenAuth
{
    /// <summary>
    /// 切换账号认证 - 结果
    /// </summary>
    public class SwitchedAccountAuthenticateResultModel
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 访问令牌（加密）
        /// </summary>
        public string EncryptedAccessToken { get; set; }

        /// <summary>
        /// 过期时间（单位:秒）
        /// </summary>
        public int ExpireInSeconds { get; set; }
    }
}