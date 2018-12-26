using Abp.Runtime.Caching;

namespace Vapps.MultiTenancy
{
    public static class TenantCacheManagerExtensions
    {
        public static ITypedCache<int, VappsTenantCacheItem> GetTenantCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<int, VappsTenantCacheItem>(VappsTenantCacheItem.CacheName);
        }

        public static ITypedCache<string, int?> GetTenantByNameCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, int?>(VappsTenantCacheItem.ByNameCacheName);
        }
    }
}
