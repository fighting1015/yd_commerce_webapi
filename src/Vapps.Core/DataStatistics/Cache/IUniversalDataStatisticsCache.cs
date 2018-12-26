using System;
using System.Threading.Tasks;

namespace Vapps.DataStatistics.Cache
{
    public interface IUniversalDataStatisticsCache
    {
        Task<UniversalDataStatisticsCacheItem> GetAsync(DateTime date, UniversalDataType type);

        Task<UniversalDataStatisticsCacheItem> GetOrNullAsync(DateTime date, UniversalDataType type);
    }
}
