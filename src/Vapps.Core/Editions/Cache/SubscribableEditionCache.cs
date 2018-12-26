using Abp;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using System.Threading.Tasks;
using Vapps.MultiTenancy;

namespace Vapps.Editions.Cache
{
    public class SubscribableEditionCache : ISubscribableEditionCache, ISingletonDependency,
        IEventHandler<EntityChangedEventData<SubscribableEdition>>
    {
        private readonly TenantManager _tenantManager;
        private readonly EditionManager _editionManager;

        private readonly ICacheManager _cacheManager;
        private readonly IObjectMapper _objectMapper;

        public SubscribableEditionCache(TenantManager tenantManager,
            EditionManager editionManager,
            ICacheManager cacheManager,
            IObjectMapper objectMapper)
        {
            this._tenantManager = tenantManager;
            this._editionManager = editionManager;
            this._cacheManager = cacheManager;
            this._objectMapper = objectMapper;
        }

        public async Task<SubscribableEditionCacheItem> GetAsync(int id)
        {
            var cacheItem = await GetOrNullAsync(id);
            if (cacheItem == null)
            {
                throw new AbpException("There is no edition with given id: " + id);
            }

            return cacheItem;
        }

        [UnitOfWork]
        public virtual async Task<SubscribableEditionCacheItem> GetDefaultAsync()
        {
            return await _cacheManager.GetCache(SubscribableEditionCacheItem.CacheName)
             .GetAsync(SubscribableEditionCacheItem.DefaultEditionCacheName, async () =>
              {
                  var edition = await _editionManager.GetDefaultEditionAsync();
                  if (edition == null)
                  {
                      return null;
                  }

                  return CreateCacheItem(edition);
              });
        }

        [UnitOfWork]
        public virtual async Task<SubscribableEditionCacheItem> GetHighestAsync()
        {
            return await _cacheManager.GetCache(SubscribableEditionCacheItem.CacheName)
            .GetAsync(SubscribableEditionCacheItem.HighestEditionCacheName, async () =>
            {
                var edition = await _editionManager.GetHighestEditionAsync();
                if (edition == null)
                {
                    return null;
                }

                return CreateCacheItem(edition);
            });
        }

        public async Task<SubscribableEditionCacheItem> GetOrNullAsync(int id)
        {
            return await _cacheManager.GetCache(SubscribableEditionCacheItem.CacheName)
              .GetAsync(id.ToString(), async () =>
               {
                   var edition = (SubscribableEdition)await _editionManager.FindByIdAsync(id);
                   if (edition == null)
                   {
                       return null;
                   }

                   return CreateCacheItem(edition);
               });
        }

        public SubscribableEditionCacheItem CreateCacheItem(SubscribableEdition edition)
        {
            return _objectMapper.Map<SubscribableEditionCacheItem>(edition);
        }

        public virtual void HandleEvent(EntityChangedEventData<SubscribableEdition> eventData)
        {
            _cacheManager.GetCache(SubscribableEditionCacheItem.CacheName)
                .Remove(eventData.Entity.Id.ToString());

            _cacheManager.GetCache(SubscribableEditionCacheItem.CacheName)
                .Remove(SubscribableEditionCacheItem.DefaultEditionCacheName);

            _cacheManager.GetCache(SubscribableEditionCacheItem.CacheName)
                .Remove(SubscribableEditionCacheItem.HighestEditionCacheName);
        }
    }
}
