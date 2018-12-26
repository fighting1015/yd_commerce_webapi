namespace Vapps.Web.Authentication.TwoFactor
{
    public class AuthenticateResultCacheItem
    {
        public const string CacheName = "AppAuthenticateResultCache";

        public string AccessToken { get; set; }

        public string EncryptedAccessToken { get; set; }

        public int ExpireInSeconds { get; set; }

        public long UserId { get; set; }

        public int? TenantId { get; set; }
    }
}
