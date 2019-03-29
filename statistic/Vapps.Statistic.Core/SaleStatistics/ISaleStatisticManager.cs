using Abp.Domain.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders;

namespace Vapps.Statistic.SaleStatistics
{
    public interface ISaleStatisticManager
    {
        IRepository<SaleStatistic, long> SaleStatisticRepository { get; }

        IQueryable<SaleStatistic> SaleStatistics { get; }

        #region SaleStatistic

        /// <summary>
        /// 根据id查找销售统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SaleStatistic> FindByIdAsync(long id);

        /// <summary>
        /// 根据时间获取销售数据统计
        /// </summary>
        /// <param name="date"></param>
        /// <param name="productId"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        Task<SaleStatistic> FindStatisticsByDate(DateTime date, int productId, OrderSource channel);

        /// <summary>
        /// 根据id获取销售统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<SaleStatistic> GetByIdAsync(long id);

        /// <summary>
        /// 添加销售统计
        /// </summary>
        /// <param name="statistic"></param>
        Task CreateAsync(SaleStatistic statistic);

        /// <summary>
        /// 修改销售统计
        /// </summary>
        /// <param name="statistic"></param>
        Task UpdateAsync(SaleStatistic statistic);

        /// <summary>
        /// 删除销售统计
        /// </summary>
        /// <param name="statistic"></param>
        Task DeleteAsync(SaleStatistic statistic);

        /// <summary>
        /// 删除销售统计
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion
    }
}
