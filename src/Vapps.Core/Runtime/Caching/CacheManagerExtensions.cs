using Abp.Runtime.Caching;
using Vapps.Identity.Cache;

namespace Vapps.Runtime.Caching
{
    public static class AbpZeroCacheManagerExtensions
    {
        public static ITypedCache<string, VerificationCodeCacheItem> GetVerificationCodeCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, VerificationCodeCacheItem>(VerificationCodeCacheItem.CacheName);
        }
    }
}
