using System;

namespace Vapps.DataStatistics
{
    public class AccessDailyStatisticsSingleJobArgs
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 数据统计日期
        /// </summary>
        public DateTime Date { get; set; }
    }
}
