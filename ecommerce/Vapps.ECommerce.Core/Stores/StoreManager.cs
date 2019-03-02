﻿using Abp.Domain.Repositories;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Stores
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
        /// <param name="logistics"></param>
        public virtual async Task CreateAsync(Store logistics)
        {
            await StoreRepository.InsertAsync(logistics);
        }

        /// <summary>
        /// 更新店铺
        /// </summary>
        /// <param name="logistics"></param>
        public virtual async Task UpdateAsync(Store logistics)
        {
            await StoreRepository.UpdateAsync(logistics);
        }

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="logistics"></param>
        public virtual async Task DeleteAsync(Store logistics)
        {
            await StoreRepository.DeleteAsync(logistics);
        }

        /// <summary>
        /// 删除店铺
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(int id)
        {
            var logistics = await StoreRepository.FirstOrDefaultAsync(id);

            if (logistics != null)
                await StoreRepository.DeleteAsync(logistics);
        }

        #endregion
    }
}
