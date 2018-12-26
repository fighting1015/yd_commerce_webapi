using Abp.AutoMapper;
using System;

namespace Vapps.DataStatistics.Cache
{
    [AutoMapFrom(typeof(UniversalDataStatistics))]
    public class UniversalDataStatisticsCacheItem
    {
        public static string CacheName = "UniversalDataStatistics";

        public virtual long Id { get; set; }

        public virtual int TenantId { get; set; }

        public virtual DateTime Date { get; set; }

        public string DataStatistics { get; set; }

        public UniversalDataType DataType { get; set; }

        public virtual DateTime CreationTime { get; set; }
    }
}
