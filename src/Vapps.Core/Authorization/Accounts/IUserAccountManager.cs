using Abp.Authorization.Users;
using Abp.Domain.Repositories;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Users;
using Vapps.ExternalAuthentications;

namespace Vapps.Authorization.Accounts
{
    public interface IUserAccountManager
    {
        IRepository<Account, long> AccountRepository { get; }

        IQueryable<Account> Accounts { get; }

        /// <summary>
        /// 根据id获取账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Account> GetByIdAsync(long id);

        /// <summary>
        /// 根据userid获取账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Account> GetByUserIdAsync(long userId);

        /// <summary>
        /// 根据userid获取账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<Account> GetByUserIdAsync(int? tanantId, long userId);

        /// <summary>
        /// 根据userid获取账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Account GetByUserId(int? tanantId, long userId);

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="account"></param>
        Task CreateAsync(Account account);

        /// <summary>
        /// 更新账户
        /// </summary>
        /// <param name="account"></param>
        Task UpdateAsync(Account account);

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="account"></param>
        Task DeleteAsync(Account account);

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="account"></param>
        Task DeleteAsync(long accountId);

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="account"></param>
        Task DeleteByUserIdAsync(int? tenantId, long userId);

        /// <summary>
        /// 创建或更新账户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="externalInfo"></param>
        /// <returns></returns>
        Task CreateOrUpdateAccountAsync(User user, ExternalLoginUserInfo externalInfo);
    }
}
