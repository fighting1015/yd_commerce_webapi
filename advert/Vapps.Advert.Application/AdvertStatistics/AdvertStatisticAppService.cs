using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Advert.AdvertStatistics.Dto;
using Vapps.Dto;

namespace Vapps.Advert.AdvertStatistics
{

    public class AdvertStatisticAppService : VappsAppServiceBase, IAdvertStatisticAppService
    {
        private readonly IAdvertDailyStatisticManager _advertDailyStatisticManager;

        public AdvertStatisticAppService(IAdvertDailyStatisticManager advertDailyStatisticManager)
        {
            this._advertDailyStatisticManager = advertDailyStatisticManager;
        }

        /// <summary>
        /// 获取所有广告数据统计
        /// </summary>
        /// <returns></returns>
        public virtual async Task<PagedResultDto<DailyStatisticDto>> GetDailyStatistics(GetDailyStatisticsInput input)
        {
            var query = _advertDailyStatisticManager
                .AdvertDailyStatistics
                .WhereIf(!input.AccountIds.IsNullOrEmpty(), r => input.AccountIds.Contains(r.AdvertAccountId))
                .WhereIf(!input.AdvertChannels.IsNullOrEmpty(), r => input.AdvertChannels.Contains(r.AdvertAccount.Channel))
                .WhereIf(!input.ProductIds.IsNullOrEmpty(), r => input.ProductIds.Contains(r.ProductId))
                .WhereIf(input.StatisticOn.FormDateNotEmpty(), r => (r.StatisticOn != null && r.StatisticOn >= input.StatisticOn.FormDate))
                .WhereIf(input.StatisticOn.ToDateNotEmpty(), r => (r.StatisticOn != null && r.StatisticOn <= input.StatisticOn.ToDate));

            var daliyStatisticCount = await query.CountAsync();

            var daliyStatistics = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var dailtStatisticListDtos = ObjectMapper.Map<List<DailyStatisticDto>>(daliyStatistics);
            return new PagedResultDto<DailyStatisticDto>(
                daliyStatisticCount,
                dailtStatisticListDtos);
        }

        /// <summary>
        /// 获取广告数据统计详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<DailyStatisticDto> GetDailyStatisticForEdit(NullableIdDto<long> input)
        {
            DailyStatisticDto dailyStatisticDto;

            if (input.Id.HasValue && input.Id > 0) //Editing existing store?
            {
                var dailyStatistic = await _advertDailyStatisticManager.GetByIdAsync(input.Id.Value);
                dailyStatisticDto = ObjectMapper.Map<DailyStatisticDto>(dailyStatistic);
            }
            else
            {
                dailyStatisticDto = new DailyStatisticDto();
            }

            return dailyStatisticDto;
        }

        /// <summary>
        /// 创建或更新广告数据统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<EntityDto<long>> CreateOrUpdateDailyStatistic(DailyStatisticDto input)
        {
            AdvertDailyStatistic dailyStatistic;
            if (input.Id > 0)
            {
                dailyStatistic = await UpdateAdvertDailyStatisticAsync(input);
            }
            else
            {
                dailyStatistic = await CreateAdvertDailyStatisticAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long> { Id = dailyStatistic.Id };
        }

        /// <summary>
        /// 删除广告数据统计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task DeleteDailyStatistic(BatchInput<int> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _advertDailyStatisticManager.DeleteAsync(id);
            }
        }

        /// <summary>
        /// 获取广告数据统计条目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual List<DailyStatisticItemDto> GetDailyStatisticItems(GetDailyStatisticItemsInput input)
        {
            if (input.AdvertDailyStatisticId == 0)
                return new List<DailyStatisticItemDto>();

            var advertStatisticItems = _advertDailyStatisticManager.AdvertDailyStatisticItems.Where(si => si.AdvertDailyStatisticId == input.AdvertDailyStatisticId);

            if (advertStatisticItems == null)
                return new List<DailyStatisticItemDto>();

            return ObjectMapper.Map<List<DailyStatisticItemDto>>(advertStatisticItems);
        }

        /// <summary>
        /// 创建或更新广告数据条目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public virtual async Task<EntityDto<long>> CreateOrUpdateDailyStatisticItem(DailyStatisticItemDto input)
        {
            AdvertDailyStatisticItem dailyStatisticItem;
            if (input.Id > 0)
            {
                dailyStatisticItem = await UpdateAdvertDailyStatisticItemAsync(input);
            }
            else
            {
                dailyStatisticItem = await CreateAdvertDailyStatisticItemAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long> { Id = dailyStatisticItem.Id };
        }

        #region Utilities

        /// <summary>
        /// 创建广告账户每日分析
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<AdvertDailyStatistic> CreateAdvertDailyStatisticAsync(DailyStatisticDto input)
        {
            var dailyStatistic = ObjectMapper.Map<AdvertDailyStatistic>(input);
            await _advertDailyStatisticManager.CreateAsync(dailyStatistic);

            return dailyStatistic;
        }

        /// <summary>
        /// 更新广告账户每日分析
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<AdvertDailyStatistic> UpdateAdvertDailyStatisticAsync(DailyStatisticDto input)
        {
            var dailyStatistic = await _advertDailyStatisticManager.GetByIdAsync(input.Id);

            ObjectMapper.Map(input, dailyStatistic);

            await _advertDailyStatisticManager.UpdateAsync(dailyStatistic);

            return dailyStatistic;
        }

        /// <summary>
        /// 创建广告账户每日分析条目
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<AdvertDailyStatisticItem> CreateAdvertDailyStatisticItemAsync(DailyStatisticItemDto input)
        {
            var dailyStatisticItem = ObjectMapper.Map<AdvertDailyStatisticItem>(input);
            await _advertDailyStatisticManager.CreateItemAsync(dailyStatisticItem);

            return dailyStatisticItem;
        }

        /// <summary>
        /// 更新广告账户每日分析条目
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<AdvertDailyStatisticItem> UpdateAdvertDailyStatisticItemAsync(DailyStatisticItemDto input)
        {
            var dailyStatisticItem = await _advertDailyStatisticManager.GetItemByIdAsync(input.Id);

            ObjectMapper.Map(input, dailyStatisticItem);

            await _advertDailyStatisticManager.UpdateItemAsync(dailyStatisticItem);

            return dailyStatisticItem;
        }

        #endregion
    }
}
