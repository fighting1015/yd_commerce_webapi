using Abp.Runtime.Caching;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vapps.DataStatistics.Cache
{
    public static class DataStatisticsCacheItemExtensions
    {
        public static ITypedCache<string, UniversalDataStatisticsCacheItem> GetUniversalDataStatisticsCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, UniversalDataStatisticsCacheItem>(UniversalDataStatisticsCacheItem.CacheName);
        }

        /// <summary>
        /// 获取访问数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<ChannelAccessData> GetAccessData(this UniversalDataStatisticsCacheItem data)
        {
            if (data == null)
                return new List<ChannelAccessData>();

            return JsonConvert.DeserializeObject<List<ChannelAccessData>>(data.DataStatistics);
        }

        /// <summary>
        /// 获取分享数据
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<ShareDataStatistics> GetShareData(this UniversalDataStatisticsCacheItem data)
        {
            if (data == null)
                return new List<ShareDataStatistics>();

            return JsonConvert.DeserializeObject<List<ShareDataStatistics>>(data.DataStatistics);
        }
    }
}
