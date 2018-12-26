using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;

namespace Vapps.Authorization.Accounts.Cache
{
    public class AccountCacheSyncronizer : IEventHandler<EntityChangedEventData<Account>>,
        IEventHandler<EntityDeletedEventData<Account>>,
        ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICacheManager _cacheManager;
        private readonly IAccountCache _accountCache;

        /// <summary>
        /// Constructor
        /// </summary>
        public AccountCacheSyncronizer(
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IAccountCache accountCache)
        {
            _unitOfWorkManager = unitOfWorkManager;
            _cacheManager = cacheManager;
            _accountCache = accountCache;
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityChangedEventData<Account> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                _accountCache.InternalCache.RemoveAsync(eventData.Entity.UserId);
            }
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<Account> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                _accountCache.InternalCache.RemoveAsync(eventData.Entity.UserId);
            }
        }
    }
}
