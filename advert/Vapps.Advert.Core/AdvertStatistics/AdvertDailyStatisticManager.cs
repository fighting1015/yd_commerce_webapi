using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.Advert.AdvertStatistics
{
    public class AdvertDailyStatisticManager : VappsDomainServiceBase, IAdvertDailyStatisticManager
    {
        #region Ctor

        public IRepository<AdvertDailyStatistic, long> AdvertDailyStatisticRepository { get; }

        public IQueryable<AdvertDailyStatistic> AdvertDailyStatistics => AdvertDailyStatisticRepository.GetAll().AsNoTracking();

        public IRepository<AdvertDailyStatisticItem, long> AdvertDailyStatisticItemRepository { get; }

        public IQueryable<AdvertDailyStatisticItem> AdvertDailyStatisticItems => AdvertDailyStatisticItemRepository.GetAll().AsNoTracking();


        public AdvertDailyStatisticManager(IRepository<AdvertDailyStatistic, long> advertDailyStatisticRepository,
            IRepository<AdvertDailyStatisticItem, long> advertDailyStatisticItemRepository)
        {
            this.AdvertDailyStatisticRepository = advertDailyStatisticRepository;
            this.AdvertDailyStatisticItemRepository = advertDailyStatisticItemRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// 创建或更新每日分析
        /// </summary>
        /// <param name="dto"></param>
        /// <returns></returns>
        public async Task InsertOrUpdateAdvertStatisticAsync(AdvertStatisticImport dto)
        {
            //统计日期
            var statistics = await GetAdvertStatistics(dto.ProductId, dto.AdvertAccountId, dto.StatisticOnUtc);
            if (statistics == null)
            {
                statistics = new AdvertDailyStatistic()
                {
                    StatisticOn = dto.StatisticOnUtc,
                    ProductId = dto.ProductId,
                    ProductName = dto.ProductName,
                    AdvertAccountId = dto.AdvertAccountId,
                };
                await CreateAsync(statistics);
            }

            statistics.ClickNum = dto.ClickNum;
            statistics.DisplayNum = dto.DisplayNum;
            statistics.TotalCost = dto.TotalCost;
            statistics.ClickPrice = dto.ClickPrice;
            statistics.ThDisplayCost = dto.ThDisplayCost;

            await UpdateAsync(statistics);
        }

        /// <summary>
        /// 根据id获取广告统计
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="accountId"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public virtual async Task<AdvertDailyStatistic> GetAdvertStatistics(long productId, long accountId, DateTime data)
        {
            return await AdvertDailyStatisticRepository.FirstOrDefaultAsync(ad => ad.ProductId == productId
                && ad.AdvertAccountId == accountId
                && ad.StatisticOn == data);
        }

        /// <summary>
        /// 根据id查找广告账户每日统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<AdvertDailyStatistic> FindByIdAsync(long id)
        {
            return await AdvertDailyStatisticRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取广告账户每日统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<AdvertDailyStatistic> GetByIdAsync(long id)
        {
            return await AdvertDailyStatisticRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加广告账户每日统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task CreateAsync(AdvertDailyStatistic statistic)
        {
            await AdvertDailyStatisticRepository.InsertAsync(statistic);
        }

        /// <summary>
        /// 更新广告账户每日统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task UpdateAsync(AdvertDailyStatistic statistic)
        {
            await AdvertDailyStatisticRepository.UpdateAsync(statistic);
        }

        /// <summary>
        /// 删除广告账户每日统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task DeleteAsync(AdvertDailyStatistic statistic)
        {
            await AdvertDailyStatisticRepository.DeleteAsync(statistic);
        }

        /// <summary>
        /// 删除广告账户每日统计
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var statistic = await AdvertDailyStatisticRepository.FirstOrDefaultAsync(id);

            if (statistic != null)
                await AdvertDailyStatisticRepository.DeleteAsync(statistic);
        }

        #endregion

        #region Advert Daily Statistic Item

        /// <summary>
        /// 根据id查找广告账户每日统计条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<AdvertDailyStatisticItem> FindItemByIdAsync(long id)
        {
            return await AdvertDailyStatisticItemRepository.FirstOrDefaultAsync(id);

        }

        /// <summary>
        /// 根据id获取广告账户每日统计条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<AdvertDailyStatisticItem> GetItemByIdAsync(long id)
        {
            return await AdvertDailyStatisticItemRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加广告账户每日统计条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task CreateItemAsync(AdvertDailyStatisticItem item)
        {
            await AdvertDailyStatisticItemRepository.InsertAsync(item);
        }

        /// <summary>
        /// 修改广告账户每日统计条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task UpdateItemAsync(AdvertDailyStatisticItem item)
        {
            await AdvertDailyStatisticItemRepository.UpdateAsync(item);
        }

        /// <summary>
        /// 删除广告账户每日统计条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task DeleteItemAsync(AdvertDailyStatisticItem item)
        {
            await AdvertDailyStatisticItemRepository.DeleteAsync(item);
        }

        /// <summary>
        /// 删除广告账户每日统计条目
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteItemAsync(long id)
        {
            var item = await AdvertDailyStatisticItemRepository.FirstOrDefaultAsync(id);

            if (item != null)
                await AdvertDailyStatisticItemRepository.DeleteAsync(item);
        }

        #endregion
    }
}
