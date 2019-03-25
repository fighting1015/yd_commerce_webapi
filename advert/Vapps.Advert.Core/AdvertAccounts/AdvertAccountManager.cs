using Abp.Domain.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Advert.AdvertAccounts
{

    public class AdvertAccountManager : VappsDomainServiceBase, IAdvertAccountManager
    {
        #region Ctor

        public IRepository<AdvertAccount, long> AdvertAccountRepository { get; }

        public IQueryable<AdvertAccount> AdvertAccounts => AdvertAccountRepository.GetAll().AsNoTracking();

        public AdvertAccountManager(IRepository<AdvertAccount, long> storeRepository)
        {
            this.AdvertAccountRepository = storeRepository;
        }

        #endregion

        #region Method

        /// <summary>
        /// 根据id查找广告账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<AdvertAccount> FindByIdAsync(long id)
        {
            return await AdvertAccountRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取广告账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<AdvertAccount> GetByIdAsync(long id)
        {
            return await AdvertAccountRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加广告账户
        /// </summary>
        /// <param name="store"></param>
        public virtual async Task CreateAsync(AdvertAccount store)
        {
            await AdvertAccountRepository.InsertAsync(store);
        }

        /// <summary>
        /// 更新广告账户
        /// </summary>
        /// <param name="store"></param>
        public virtual async Task UpdateAsync(AdvertAccount store)
        {
            await AdvertAccountRepository.UpdateAsync(store);
        }

        /// <summary>
        /// 删除广告账户
        /// </summary>
        /// <param name="store"></param>
        public virtual async Task DeleteAsync(AdvertAccount store)
        {
            await AdvertAccountRepository.DeleteAsync(store);
        }

        /// <summary>
        /// 删除广告账户
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var store = await AdvertAccountRepository.FirstOrDefaultAsync(id);

            if (store != null)
                await AdvertAccountRepository.DeleteAsync(store);
        }

        #endregion
    }
}
