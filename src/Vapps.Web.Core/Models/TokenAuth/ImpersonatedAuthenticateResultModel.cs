namespace Vapps.Web.Models.TokenAuth
{
    public class ImpersonatedAuthenticateResultModel
    {
        /// <summary>
        /// 访问令牌
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// 访问令牌(加密)
        /// </summary>
        public string EncryptedAccessToken { get; set; }

        /// <summary>
        /// 过期时间(秒)
        /// </summary>
        public int ExpireInSeconds { get; set; }
    }
}