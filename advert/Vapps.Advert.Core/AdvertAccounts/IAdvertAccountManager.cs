using Abp.Domain.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Advert.AdvertAccounts
{
    public interface IAdvertAccountManager
    {
        IRepository<AdvertAccount, long> AdvertAccountRepository { get; }

        IQueryable<AdvertAccount> AdvertAccounts { get; }

        #region AdvertAccount

        /// <summary>
        /// 根据id查找广告账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdvertAccount> FindByIdAsync(long id);

        /// <summary>
        /// 根据id获取广告账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<AdvertAccount> GetByIdAsync(long id);

        /// <summary>
        /// 添加广告账户
        /// </summary>
        /// <param name="advertAccount"></param>
        Task CreateAsync(AdvertAccount advertAccount);

        /// <summary>
        /// 修改广告账户
        /// </summary>
        /// <param name="advertAccount"></param>
        Task UpdateAsync(AdvertAccount advertAccount);

        /// <summary>
        /// 删除广告账户
        /// </summary>
        /// <param name="advertAccount"></param>
        Task DeleteAsync(AdvertAccount advertAccount);

        /// <summary>
        /// 删除广告账户
        /// </summary>
        /// <param name="id"></param>
        Task DeleteAsync(long id);

        #endregion
    }
}
