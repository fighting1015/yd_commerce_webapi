using Abp.Runtime.Caching;
using System;

namespace Vapps.DataStatistics.Cache
{
    public static class DataStatisticsAppCacheItemExtensions
    {
        public static ITypedCache<string, T> GetUniversalDataStatisticsCache<T>(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, T>(DataStatisticsCacheConst.CacheName);
        }

        public static string GetCacheKey(string cacheName, int tenantId, DateTime? date = null)
        {
            if (date == null)
                date = new DateTime();

            return $"{cacheName}-{tenantId}-{date.Value.ToString("d")}";
        }

        public static string GetCacheKey(string cacheName, int tenantId)
        {
            return $"{cacheName}-{tenantId}";
        }
    }
}
