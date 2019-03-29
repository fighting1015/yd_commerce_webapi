using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders;

namespace Vapps.Statistic.ShipmentStatistics
{
    public class ShipmentStatisticManager : VappsDomainServiceBase, IShipmentStatisticManager
    {
        #region Ctor

        public IRepository<ShipmentStatistic, long> ShipmentStatisticRepository { get; }

        public IQueryable<ShipmentStatistic> ShipmentStatistics => ShipmentStatisticRepository.GetAll().AsNoTracking();

        public ShipmentStatisticManager(IRepository<ShipmentStatistic, long> statisticRepository)
        {
            this.ShipmentStatisticRepository = statisticRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// 根据id查找销售统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ShipmentStatistic> FindByIdAsync(long id)
        {
            return await ShipmentStatisticRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据时间获取销售数据统计
        /// </summary>
        /// <param name="date"></param>
        /// <param name="productId"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        public virtual async Task<ShipmentStatistic> FindStatisticsByDate(DateTime date, int productId, OrderSource channel)
        {
            return await ShipmentStatisticRepository
                .FirstOrDefaultAsync(ss => ss.Date == date && ss.ProductId == productId && ss.Channel == channel);
        }

        /// <summary>
        /// 根据id获取销售统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<ShipmentStatistic> GetByIdAsync(long id)
        {
            return await ShipmentStatisticRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加销售统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task CreateAsync(ShipmentStatistic statistic)
        {
            await ShipmentStatisticRepository.InsertAsync(statistic);
        }

        /// <summary>
        /// 更新销售统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task UpdateAsync(ShipmentStatistic statistic)
        {
            await ShipmentStatisticRepository.UpdateAsync(statistic);
        }

        /// <summary>
        /// 删除销售统计
        /// </summary>
        /// <param name="statistic"></param>
        public virtual async Task DeleteAsync(ShipmentStatistic statistic)
        {
            await ShipmentStatisticRepository.DeleteAsync(statistic);
        }

        /// <summary>
        /// 删除销售统计
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var statistic = await ShipmentStatisticRepository.FirstOrDefaultAsync(id);

            if (statistic != null)
                await ShipmentStatisticRepository.DeleteAsync(statistic);
        }

        #endregion
    }
}
