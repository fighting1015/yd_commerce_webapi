using Abp.Runtime.Validation;
using System.Collections.Generic;
using Vapps.Advert.AdvertAccounts;
using Vapps.Dto;

namespace Vapps.Advert.AdvertStatistics.Dto
{
    public class GetDailyStatisticsInput : PagedAndSortedInputDto, IShouldNormalize
    {
        /// <summary>
        /// 渠道
        /// </summary>
        public AdvertChannel[] AdvertChannels { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public List<long> ProductIds { get; set; }

        /// <summary>
        /// 账户Id
        /// </summary>
        public List<long> AccountIds { get; set; }

        /// <summary>
        /// 统计时间(Utc)
        /// </summary>
        public DateRangeDto StatisticOn { get; set; }

        public void Normalize()
        {
            if (string.IsNullOrEmpty(Sorting))
            {
                Sorting = "CreationTime,Id DESC";
            }
        }
    }
}
