using Abp.Runtime.Caching;

namespace Vapps.Web.Authentication.TwoFactor
{
    public static class AuthenticateResultCacheExtension
    {
        public static ITypedCache<string, AuthenticateResultCacheItem> GetAuthenticateResultCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, AuthenticateResultCacheItem>(AuthenticateResultCacheItem.CacheName);
        }
    }
}
