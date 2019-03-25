using Abp.Application.Services.Dto;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Advert.AdvertStatistics.Dto;
using Vapps.Dto;

namespace Vapps.Advert.AdvertStatistics
{
    public interface IAdvertStatisticAppService
    {
        /// <summary>
        /// 获取所有广告数据统计
        /// </summary>
        /// <returns></returns>
        Task<PagedResultDto<DailyStatisticDto>> GetDailyStatistics(GetDailyStatisticsInput input);

        /// <summary>
        /// 获取广告数据统计详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<DailyStatisticDto> GetDailyStatisticForEdit(NullableIdDto<long> input);

        /// <summary>
        /// 创建或更新广告数据统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EntityDto<long>> CreateOrUpdateDailyStatistic(DailyStatisticDto input);

        /// <summary>
        /// 删除广告数据统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task DeleteDailyStatistic(BatchInput<int> input);

        /// <summary>
        /// 获取广告数据统计条目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        List<DailyStatisticItemDto> GetDailyStatisticItems(GetDailyStatisticItemsInput input);

        /// <summary>
        /// 创建或更新广告数据条目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<EntityDto<long>> CreateOrUpdateDailyStatisticItem(DailyStatisticItemDto input);
    }
}
