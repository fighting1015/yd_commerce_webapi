using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Shippings
{
    public class LogisticsManager : VappsDomainServiceBase, ILogisticsManager
    {
        #region Ctor

        public IRepository<Logistics, int> LogisticsRepository { get; }

        public IQueryable<Logistics> Logisticses => LogisticsRepository.GetAll().AsNoTracking();

        public IRepository<TenantLogistics, int> TenantLogisticsRepository { get; }

        public IQueryable<TenantLogistics> TenantLogisticses => TenantLogisticsRepository.GetAll().AsNoTracking();

        public LogisticsManager(IRepository<Logistics, int> logisticsRepository,
            IRepository<TenantLogistics, int> tenantLogisticsRepository)
        {
            this.LogisticsRepository = logisticsRepository;
            this.TenantLogisticsRepository = tenantLogisticsRepository;
        }

        #endregion

        #region Logistics

        /// <summary>
        /// 根据id查找物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Logistics> FindByIdAsync(int id)
        {
            return await LogisticsRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据name查找快递
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<Logistics> FindByNameAsync(string name)
        {
            return await LogisticsRepository.FirstOrDefaultAsync(l => l.Name == name);
        }

        /// <summary>
        /// 根据Key查找物流
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public virtual async Task<Logistics> FindByKeyAsync(string key)
        {
            return await LogisticsRepository.FirstOrDefaultAsync(l => l.Key == key);
        }

        /// <summary>
        /// 根据id获取物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Logistics> GetByIdAsync(int id)
        {
            return await LogisticsRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加物流
        /// </summary>
        /// <param name="logistics"></param>
        public virtual async Task CreateAsync(Logistics logistics)
        {
            await LogisticsRepository.InsertAsync(logistics);
        }

        /// <summary>
        /// 更新物流
        /// </summary>
        /// <param name="logistics"></param>
        public virtual async Task UpdateAsync(Logistics logistics)
        {
            await LogisticsRepository.UpdateAsync(logistics);
        }

        /// <summary>
        /// 删除物流
        /// </summary>
        /// <param name="logistics"></param>
        public virtual async Task DeleteAsync(Logistics logistics)
        {
            await LogisticsRepository.DeleteAsync(logistics);
        }

        /// <summary>
        /// 删除物流
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(int id)
        {
            var logistics = await LogisticsRepository.FirstOrDefaultAsync(id);

            if (logistics != null)
                await LogisticsRepository.DeleteAsync(logistics);
        }

        #endregion

        #region TenantLogistics

        /// <summary>
        /// 根据物流id查找自选物流
        /// </summary>
        /// <param name="logisticsId"></param>
        /// <returns></returns>
        public virtual async Task<TenantLogistics> FindTenantLogisticsByLogisticsIdAsync(int logisticsId)
        {
            return await TenantLogisticsRepository.FirstOrDefaultAsync(tl => tl.LogisticsId == logisticsId);
        }

        /// <summary>
        /// 根据id查找自选物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TenantLogistics> FindTenantLogisticsByIdAsync(int id)
        {
            return await TenantLogisticsRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取自选物流
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<TenantLogistics> GetTenantLogisticsByIdAsync(int id)
        {
            return await TenantLogisticsRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加自选物流
        /// </summary>
        /// <param name="tenantLogistics"></param>
        public virtual async Task CreateTenantLogisticsAsync(TenantLogistics tenantLogistics)
        {
            await TenantLogisticsRepository.InsertAsync(tenantLogistics);
        }

        /// <summary>
        /// 更新自选物流
        /// </summary>
        /// <param name="tenantLogistics"></param>
        public virtual async Task UpdateTenantLogisticsAsync(TenantLogistics tenantLogistics)
        {
            await TenantLogisticsRepository.UpdateAsync(tenantLogistics);
        }

        /// <summary>
        /// 删除自选物流
        /// </summary>
        /// <param name="tenantLogistics"></param>
        public virtual async Task DeleteTenantLogisticsAsync(TenantLogistics tenantLogistics)
        {
            await TenantLogisticsRepository.DeleteAsync(tenantLogistics);
        }

        /// <summary>
        /// 删除自选物流
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteTenantLogisticsAsync(int id)
        {
            var tenantLogistics = await TenantLogisticsRepository.FirstOrDefaultAsync(id);

            if (tenantLogistics != null)
                await TenantLogisticsRepository.DeleteAsync(tenantLogistics);
        }


        /// <summary>
        /// 删除自选物流
        /// </summary>
        /// <param name="logisticsId"></param>
        public virtual async Task DeleteTenantLogisticsByLogisticsIdAsync(int logisticsId)
        {
            var tenantLogistics = await TenantLogisticsRepository.FirstOrDefaultAsync(tl => tl.LogisticsId == logisticsId);

            if (tenantLogistics != null)
                await TenantLogisticsRepository.DeleteAsync(tenantLogistics);
        }

        #endregion
    }
}
