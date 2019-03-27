using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Shippings
{
    public interface ILogisticsManager
    {
        IRepository<Logistics, int> LogisticsRepository { get; }
        IQueryable<Logistics> Logisticses { get; }

        IRepository<TenantLogistics, int> TenantLogisticsRepository { get; }
        IQueryable<TenantLogistics> TenantLogisticses { get; }

        #region Logistics

        /// <summary>
        /// 根据id查找快递
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Logistics> FindByIdAsync(int id);

        /// <summary>
        /// 根据name查找快递
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        Task<Logistics> FindByNameAsync(string name);

        /// <summary>
        /// 根据Key查找物流
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<Logistics> FindByKeyAsync(string key);

        /// <summary>
        /// 根据id获取快递
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Logistics> GetByIdAsync(int id);

        /// <summary>
        /// 添加快递
        /// </summary>
        /// <param name="Logistics"></param>
        Task CreateAsync(Logistics Logistics);

        /// <summary>
        /// 修改快递
        /// </summary>
        /// <param name="Logistics"></param>
        Task UpdateAsync(Logistics Logistics);

        /// <summary>
        /// 删除快递
        /// </summary>
        /// <param name="Logistics"></param>
        Task DeleteAsync(Logistics Logistics);

        /// <summary>
        /// 删除快递
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(int id);

        #endregion

        #region TenantLogistics

        /// <summary>
        /// 根据物流id查找订单条目
        /// </summary>
        /// <param name="logisticsId"></param>
        /// <returns></returns>
        Task<TenantLogistics> FindTenantLogisticsByLogisticsIdAsync(int logisticsId);

        /// <summary>
        /// 根据id查找租户快递
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TenantLogistics> FindTenantLogisticsByIdAsync(int id);

        /// <summary>
        /// 根据id获取租户快递
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TenantLogistics> GetTenantLogisticsByIdAsync(int id);

        /// <summary>
        /// 添加租户快递
        /// </summary>
        /// <param name="tenantLogistics"></param>
        Task CreateTenantLogisticsAsync(TenantLogistics tenantLogistics);

        /// <summary>
        /// 修改租户快递
        /// </summary>
        /// <param name="tenantLogistics"></param>
        Task UpdateTenantLogisticsAsync(TenantLogistics tenantLogistics);

        /// <summary>
        /// 删除租户快递
        /// </summary>
        /// <param name="tenantLogistics"></param>
        Task DeleteTenantLogisticsAsync(TenantLogistics tenantLogistics);

        /// <summary>
        /// 删除租户快递
        /// </summary>
        /// <param name="id"></param>
        Task DeleteTenantLogisticsAsync(int id);

        /// <summary>
        /// 删除订单条目
        /// </summary>
        /// <param name="logisticsId"></param>
        Task DeleteTenantLogisticsByLogisticsIdAsync(int logisticsId);

        #endregion
    }
}
