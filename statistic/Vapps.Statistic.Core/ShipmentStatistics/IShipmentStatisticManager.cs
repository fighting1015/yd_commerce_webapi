using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders;

namespace Vapps.Statistic.ShipmentStatistics
{
    public interface IShipmentStatisticManager
    {
        IRepository<ShipmentStatistic, long> ShipmentStatisticRepository { get; }

        IQueryable<ShipmentStatistic> ShipmentStatistics { get; }

        #region ShipmentStatistic

        /// <summary>
        /// 根据id查找物流统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ShipmentStatistic> FindByIdAsync(long id);

        /// <summary>
        /// 根据时间获取物流统计
        /// </summary>
        /// <param name="date"></param>
        /// <param name="productId"></param>
        /// <param name="channel"></param>
        /// <returns></returns>
        Task<ShipmentStatistic> FindStatisticsByDate(DateTime date, int productId, OrderSource channel);

        /// <summary>
        /// 根据id获取物流统计
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<ShipmentStatistic> GetByIdAsync(long id);

        /// <summary>
        /// 添加物流统计
        /// </summary>
        /// <param name="statistic"></param>
        Task CreateAsync(ShipmentStatistic statistic);

        /// <summary>
        /// 修改物流统计
        /// </summary>
        /// <param name="statistic"></param>
        Task UpdateAsync(ShipmentStatistic statistic);

        /// <summary>
        /// 删除物流统计
        /// </summary>
        /// <param name="statistic"></param>
        Task DeleteAsync(ShipmentStatistic statistic);

        /// <summary>
        /// 删除物流统计
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion
    }
}
