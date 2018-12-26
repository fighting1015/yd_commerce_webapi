using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using System.Threading.Tasks;

namespace Vapps.Payments.Cache
{
    public class SubscriptionPaymentCache : ISubscriptionPaymentCache, ISingletonDependency,
         IEventHandler<EntityChangedEventData<SubscriptionPayment>>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IPaymentIdCache _paymentIdCache;
        private readonly ISubscriptionPaymentManager _pymentManager;
        private readonly IObjectMapper _objectMapper;

        public SubscriptionPaymentCache(ICacheManager cacheManager,
            IPaymentIdCache paymentIdCache,
            ISubscriptionPaymentManager pymentManager,
            IObjectMapper objectMapper)
        {
            _cacheManager = cacheManager;
            _paymentIdCache = paymentIdCache;
            _pymentManager = pymentManager;
            _objectMapper = objectMapper;
        }

        public virtual async Task<SubscriptionPaymentCacheItem> GetCacheItemOrNullAsync(string paymentId)
        {
            return await _cacheManager.GetCache(SubscriptionPaymentCacheItem.CacheName)
                .GetAsync(paymentId, async () =>
                {
                    var payment = await _pymentManager.FindByPaymentIdAsync(paymentId);
                    if (payment == null)
                        return null;

                    return CreateCacheItem(payment);
                });
        }

        public async Task AddCacheItemAsync(SubscriptionPaymentCacheItem item)
        {
            await _cacheManager.GetCache(SubscriptionPaymentCacheItem.CacheName).SetAsync(item.PaymentId, item);
        }

        public async Task RemoveCacheItemAsync(string paymentId)
        {
            var cacheItem = GetCacheItemOrNullAsync(paymentId);
            if (cacheItem == null)
            {
                return;
            }

            await _cacheManager.GetCache(SubscriptionPaymentCacheItem.CacheName).RemoveAsync(paymentId);
        }

        public void HandleEvent(EntityChangedEventData<SubscriptionPayment> eventData)
        {
            _cacheManager
             .GetCache(SubscriptionPaymentCacheItem.CacheName)
             .Remove(eventData.Entity.PaymentId);
        }

        private SubscriptionPaymentCacheItem CreateCacheItem(SubscriptionPayment payment)
        {
            return _objectMapper.Map<SubscriptionPaymentCacheItem>(payment);
        }
    }
}
