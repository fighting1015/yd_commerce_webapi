using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.Stores
{
    public class StoreManager : VappsDomainServiceBase, IStoreManager
    {
        #region Ctor

        public IRepository<Store, int> StoreRepository { get; }

        public IQueryable<Store> Stores => StoreRepository.GetAll().AsNoTracking();

        private readonly IAbpSession _abpSession;

        public StoreManager(IRepository<Store, int> storeRepository,
            IAbpSession abpSession)
        {
            this.StoreRepository = storeRepository;
            this._abpSession = abpSession;
        }

        #endregion

        #region Method

        /// <summary>
        /// 根据id查找店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Store> FindByIdAsync(int id)
        {
            return await StoreRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取店铺
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Store> GetByIdAsync(int id)
        {
            return await StoreRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加店铺
        /// </summary>
        /// <param name="store"></param>
        public virtual async Task CreateAsync(Store store)
        {
            await StoreRepository.InsertAsync(store);
        }

        /// <summary>
        /// 更新店铺
        /// </summary>
        /// <param name="Store"></param>
        public virtual async Task UpdateAsync(Store store)
        {
            await StoreRepository.UpdateAsync(store);
        }

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="Store"></param>
        public virtual async Task DeleteAsync(Store store)
        {
            await StoreRepository.DeleteAsync(store);
        }

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(int id)
        {
            var store = await StoreRepository.FirstOrDefaultAsync(id);

            if (store != null)
                await StoreRepository.DeleteAsync(store);
        }

        #endregion
    }
}
