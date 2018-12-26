using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vapps.DataStatistics
{
    public static class UniversalDataStatisticsExtension
    {
        /// <summary>
        /// 获取统计结果
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public static List<T> GetDataStatisticsList<T>(this UniversalDataStatistics data)
        {
            if (data == null)
                return new List<T>();

            return JsonConvert.DeserializeObject<List<T>>(data.DataStatistics);
        }
    }
}
