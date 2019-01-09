using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.Stores
{
    public interface IStoreManager
    {
        IRepository<Store, int> StoreRepository { get; }

        IQueryable<Store> Stores { get; }

        #region Store

        /// <summary>
        /// 根据id查找店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Store> FindByIdAsync(int id);

        /// <summary>
        /// 根据id获取店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Store> GetByIdAsync(int id);

        /// <summary>
        /// 添加店铺
        /// </summary>
        /// <param name="store"></param>
        Task CreateAsync(Store store);

        /// <summary>
        /// 修改店铺
        /// </summary>
        /// <param name="store"></param>
        Task UpdateAsync(Store store);

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="store"></param>
        Task DeleteAsync(Store store);

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(int id);

        #endregion
    }
}
