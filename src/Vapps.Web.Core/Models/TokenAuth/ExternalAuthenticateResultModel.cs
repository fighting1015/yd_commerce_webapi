namespace Vapps.Web.Models.TokenAuth
{
    public class ExternalAuthenticateResultModel
    {
        /// <summary>
        /// 租户Id 
        /// </summary>
        public long? TenantId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

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

        /// <summary>
        /// 等待激活
        /// </summary>
        public bool WaitingForActivation { get; set; }

        /// <summary>
        /// 需要补充注册
        /// </summary>
        public bool NeedSupplementary { get; set; }
    }
}