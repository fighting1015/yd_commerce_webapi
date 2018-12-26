using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.DataStatistics
{
    public interface IUniversalDataStatisticsManager
    {
        IRepository<UniversalDataStatistics, long> Repository { get; }

        IQueryable<UniversalDataStatistics> Statisticses { get; }

        /// <summary>
        /// 访问数据统计
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task AccessDataDailyStatistics(int tenantId, DateTime date);

        /// <summary>
        /// 分享数据统计
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        Task ShareDataDailyStatistics(int tenantId, DateTime date);

        /// <summary>
        /// 根据日期获取数据概况
        /// </summary>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        Task<UniversalDataStatistics> FindByDateAsync(DateTime date, UniversalDataType type);

        /// <summary>
        /// 根据id获取数据概况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<UniversalDataStatistics> GetByAsync(long id);

        /// <summary>
        /// 添加通用数据概况
        /// </summary>
        /// <param name="statistics"></param>
        Task CreateAsync(UniversalDataStatistics statistics);

        /// <summary>
        /// 修改通用数据概况
        /// </summary>
        /// <param name="statistics"></param>
        Task UpdateAsync(UniversalDataStatistics statistics);

        /// <summary>
        /// 删除通用数据概况
        /// </summary>
        /// <param name="statistics"></param>
        Task DeleteAsync(UniversalDataStatistics statistics);

        /// <summary>
        /// 删除通用数据概况
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);
    }
}
