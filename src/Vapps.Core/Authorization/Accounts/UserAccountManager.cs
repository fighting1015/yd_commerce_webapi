using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Authorization.Users;
using Vapps.ExternalAuthentications;
using Vapps.Media;
using Vapps.States;
using Vapps.States.Cache;

namespace Vapps.Authorization.Accounts
{
    public class UserAccountManager : VappsDomainServiceBase, IUserAccountManager
    {
        public IRepository<Account, long> AccountRepository { get; }

        public IQueryable<Account> Accounts => AccountRepository.GetAll().AsNoTracking();
        private readonly IStateCache _stateCache;
        private readonly IPictureManager _pictureManager;

        public UserAccountManager(IRepository<Account, long> accountRepository,
            IRepository<User, long> userRepository,
            IStateCache stateCache,
            IPictureManager pictureManager)
        {
            AccountRepository = accountRepository;
            _stateCache = stateCache;
            _pictureManager = pictureManager;
        }

        /// <summary>
        /// 根据id获取账户
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Account> GetByIdAsync(long id)
        {
            return await AccountRepository.GetAsync(id);
        }

        /// <summary>
        /// 根据userid获取账户
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<Account> GetByUserIdAsync(long userId)
        {
            return await AccountRepository.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        /// <summary>
        /// 根据userid获取账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task<Account> GetByUserIdAsync(int? tenantId, long userId)
        {
            return await AccountRepository.FirstOrDefaultAsync(a => a.TenantId == tenantId && a.UserId == userId);
        }

        /// <summary>
        /// 根据userid获取账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual Account GetByUserId(int? tenantId, long userId)
        {
            return AccountRepository.FirstOrDefault(a => a.TenantId == tenantId && a.UserId == userId);
        }

        /// <summary>
        /// 创建账户
        /// </summary>
        /// <param name="account"></param>
        public virtual async Task CreateAsync(Account account)
        {
            await AccountRepository.InsertAsync(account);
        }

        /// <summary>
        /// 更新账户资料
        /// </summary>
        /// <param name="account"></param>
        public virtual async Task UpdateAsync(Account account)
        {
            await AccountRepository.UpdateAsync(account);
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="account"></param>
        public virtual async Task DeleteAsync(Account account)
        {
            await AccountRepository.DeleteAsync(account);
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="accountId"></param>
        /// <returns></returns>
        public virtual async Task DeleteAsync(long accountId)
        {
            var account = await GetByIdAsync(accountId);
            if (account != null)
                await AccountRepository.DeleteAsync(account);
        }

        /// <summary>
        /// 删除账户
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public virtual async Task DeleteByUserIdAsync(int? tenantId, long userId)
        {
            var account = await GetByUserIdAsync(tenantId, userId);
            if (account != null)
                await AccountRepository.DeleteAsync(account);
        }

        /// <summary>
        /// 创建或更新账户
        /// </summary>
        /// <param name="user"></param>
        /// <param name="externalInfo"></param>
        /// <returns></returns>
        [UnitOfWork]
        public virtual async Task CreateOrUpdateAccountAsync(User user, ExternalLoginUserInfo externalInfo)
        {
            var userAccount = await GetByUserIdAsync(user.Id);
            if (userAccount == null)
                userAccount = new Account();

            var province = _stateCache.GetProvinceByNameOrNull(externalInfo.Province);
            var city = _stateCache.GetCityByNameOrNull(externalInfo.City);
            userAccount.NickName = externalInfo.Name;
            userAccount.TenantId = user.TenantId;
            userAccount.UserName = user.UserName;
            userAccount.UserId = user.Id;
            userAccount.EmailAddress = user.EmailAddress;
            userAccount.LastLoginTime = user.LastLoginTime;
            userAccount.ProvinceId = province?.Id ?? 0;
            userAccount.Province = province?.Name ?? string.Empty;
            userAccount.CityId = city?.Id ?? 0;
            userAccount.City = city?.Name ?? string.Empty;
            userAccount.Gender = externalInfo.Gender;
            userAccount.LastActiveTime = DateTime.UtcNow;

            if (!externalInfo.ProfilePictureUrl.IsNullOrEmpty() && userAccount.ProfilePictureUrl != externalInfo.ProfilePictureUrl)
            {
                using (UnitOfWorkManager.Current.SetTenantId(user.TenantId))
                {
                    //删除旧头像
                    await _pictureManager.DeleteAsync(userAccount.ProfilePictureId);

                    var picture = await _pictureManager.FetchPictureAsync(externalInfo.ProfilePictureUrl, (long)DefaultGroups.ProfilePicture);

                    userAccount.ProfilePictureId = picture?.Id ?? 0;
                    userAccount.ProfilePictureUrl = externalInfo.ProfilePictureUrl;
                }
            }

            if (userAccount.Id == 0)
                await CreateAsync(userAccount);
            else
                await UpdateAsync(userAccount);
        }
    }
}
