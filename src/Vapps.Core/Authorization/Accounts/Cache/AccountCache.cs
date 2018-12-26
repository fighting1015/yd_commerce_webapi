using Abp.Dependency;
using Abp.Domain.Entities.Caching;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using System.Threading.Tasks;

namespace Vapps.Authorization.Accounts.Cache
{
    public class AccountCache : EntityCache<Account, AccountCacheItem, long>, IAccountCache, ISingletonDependency
    {
        private IRepository<Account, long> _accountRepository { get; set; }
        private IUnitOfWorkManager _unitOfWorkManager { get; set; }

        public AccountCache(ICacheManager cacheManager,
            IRepository<Account, long> repository,
            IUnitOfWorkManager unitOfWorkManager,
            string cacheName = null)
            : base(cacheManager, repository, cacheName)
        {
            this._accountRepository = repository;
            this._unitOfWorkManager = unitOfWorkManager;
        }

        public override AccountCacheItem Get(long userId)
        {
            return InternalCache.Get(userId, () => GetCacheItemFromDataSource(userId));
        }

        public override Task<AccountCacheItem> GetAsync(long userId)
        {
            return InternalCache.GetAsync(userId, () => GetCacheItemFromDataSourceAsync(userId));
        }

        protected override Account GetEntityFromDataSource(long userId)
        {
            return Repository.FirstOrDefault(a => a.UserId == userId);
        }

        protected override Task<Account> GetEntityFromDataSourceAsync(long userId)
        {
            return Repository.FirstOrDefaultAsync(a => a.UserId == userId);
        }

        protected override AccountCacheItem MapToCacheItem(Account account)
        {
            if (account == null)
                return null;

            var accountCache = ObjectMapper.Map<AccountCacheItem>(account);
            return accountCache;
        }

    }
}
