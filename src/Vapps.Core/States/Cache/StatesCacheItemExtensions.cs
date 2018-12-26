using Abp.Runtime.Caching;

namespace Vapps.States.Cache
{
    public static class StatesCacheItemExtensions
    {
        public static ITypedCache<long, ProvinceCacheItem> GetProvinceCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<long, ProvinceCacheItem>(ProvinceCacheItem.CacheName);
        }

        public static ITypedCache<string, ProvinceCacheItem> GetProvinceByNameCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, ProvinceCacheItem>(ProvinceCacheItem.CacheByName);
        }

        public static ITypedCache<long, CityCacheItem> GetCityCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<long, CityCacheItem>(CityCacheItem.CacheName);
        }

        public static ITypedCache<string, CityCacheItem> GetCityByNameCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, CityCacheItem>(CityCacheItem.CacheByName);
        }

        public static ITypedCache<long, DistrictCacheItem> GetDistrictCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<long, DistrictCacheItem>(DistrictCacheItem.CacheName);
        }

        public static ITypedCache<string, DistrictCacheItem> GetDistrictByNameCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, DistrictCacheItem>(DistrictCacheItem.CacheByName);
        }
    }
}
