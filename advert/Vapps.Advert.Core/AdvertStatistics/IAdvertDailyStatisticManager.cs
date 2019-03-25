using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.Advert.AdvertStatistics
{
    public interface IAdvertDailyStatisticManager
    {
        IRepository<AdvertDailyStatistic, long> AdvertDailyStatisticRepository { get; }

        IQueryable<AdvertDailyStatistic> AdvertDailyStatistics { get; }

        IRepository<AdvertDailyStatisticItem, long> AdvertDailyStatisticItemRepository { get; }

        IQueryable<AdvertDailyStatisticItem> AdvertDailyStatisticItems { get; }


        #region Advert Daily Statistic

        /// <summary>
        /// 根据id查找广告账户每日统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdvertDailyStatistic> FindByIdAsync(long id);

        /// <summary>
        /// 根据id获取广告账户每日统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdvertDailyStatistic> GetByIdAsync(long id);

        /// <summary>
        /// 添加广告账户每日统计
        /// </summary>
        /// <param name="statistic"></param>
        Task CreateAsync(AdvertDailyStatistic statistic);

        /// <summary>
        /// 修改广告账户每日统计
        /// </summary>
        /// <param name="statistic"></param>
        Task UpdateAsync(AdvertDailyStatistic statistic);

        /// <summary>
        /// 删除广告账户每日统计
        /// </summary>
        /// <param name="statistic"></param>
        Task DeleteAsync(AdvertDailyStatistic statistic);

        /// <summary>
        /// 删除广告账户每日统计
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion

        #region Advert Daily Statistic Item

        /// <summary>
        /// 根据id查找广告账户每日统计条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdvertDailyStatisticItem> FindItemByIdAsync(long id);

        /// <summary>
        /// 根据id获取广告账户每日统计条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdvertDailyStatisticItem> GetItemByIdAsync(long id);

        /// <summary>
        /// 添加广告账户每日统计条目
        /// </summary>
        /// <param name="item"></param>
        Task CreateItemAsync(AdvertDailyStatisticItem item);

        /// <summary>
        /// 修改广告账户每日统计条目
        /// </summary>
        /// <param name="item"></param>
        Task UpdateItemAsync(AdvertDailyStatisticItem item);

        /// <summary>
        /// 删除广告账户每日统计条目
        /// </summary>
        /// <param name="item"></param>
        Task DeleteItemAsync(AdvertDailyStatisticItem item);

        /// <summary>
        /// 删除广告账户每日统计条目
        /// </summary>
        /// <param name="id"></param>
        Task DeleteItemAsync(long id);

        #endregion

    }
}
