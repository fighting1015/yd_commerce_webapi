using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders;

namespace Vapps.Statistic.SaleStatistics
{
    public class SaleStatisticManager : VappsDomainServiceBase, ISaleStatisticManager
    {
        #region Ctor

        public IRepository<SaleStatistic, long> SaleStatisticRepository { get; }

        public IQueryable<SaleStatistic> SaleStatistics => SaleStatisticRepository.GetAll().AsNoTracking();

        public SaleStatisticManager(IRepository<SaleStatistic, long> statisticRepository)
        {
            this.SaleStatisticRepository = statisticRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// 根据id查找销售统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<SaleStatistic> FindByIdAsync(long id)
        {
            return await SaleStatisticRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据时间获取销售数据统计
        /// </summary>
        /// <param name="date"></param>
        /// <param name="productId"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public virtual async Task<SaleStatistic> FindStatisticsByDate(DateTime date, int productId, OrderSource channel)
        {
            return await SaleStatisticRepository
                .FirstOrDefaultAsync(ss => ss.Date == date && ss.ProductId == productId && ss.Channel == channel);
        }

        /// <summary>
        /// 根据id获取销售统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<SaleStatistic> GetByIdAsync(long id)
        {
            return await SaleStatisticRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加销售统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task CreateAsync(SaleStatistic statistic)
        {
            await SaleStatisticRepository.InsertAsync(statistic);
        }

        /// <summary>
        /// 更新销售统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task UpdateAsync(SaleStatistic statistic)
        {
            await SaleStatisticRepository.UpdateAsync(statistic);
        }

        /// <summary>
        /// 删除销售统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task DeleteAsync(SaleStatistic statistic)
        {
            await SaleStatisticRepository.DeleteAsync(statistic);
        }

        /// <summary>
        /// 删除销售统计
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var statistic = await SaleStatisticRepository.FirstOrDefaultAsync(id);

            if (statistic != null)
                await SaleStatisticRepository.DeleteAsync(statistic);
        }

        #endregion
    }
}
