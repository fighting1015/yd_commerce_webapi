using Abp.Dependency;
using Abp.Runtime.Caching;
using Vapps.Editions;
using Vapps.Helpers;

namespace Vapps.Payments.Cache
{
    public class PaymentIdCache : IPaymentIdCache, ISingletonDependency
    {
        public const string CacheName = "PaymentIdCache";
        private readonly ICacheManager _cacheManager;

        public PaymentIdCache(ICacheManager cacheManager)
        {
            this._cacheManager = cacheManager;
        }

        public string GetCacheItem(int tenantId,
          int editionId,
          EditionPaymentType editionPaymentType,
          PaymentPeriodType? paymentPeriodType)
        {
            string key = GetCacheKey(tenantId, editionId, editionPaymentType, paymentPeriodType);

            return _cacheManager
           .GetCache(CacheName).Get(key, () =>
           {
               return CommonHelper.GenerateUniqueId();
           });
        }

        public void RemoveCacheItem(int tenantId,
            int editionId,
            EditionPaymentType editionPaymentType,
            PaymentPeriodType? paymentPeriodType)
        {
            string key = GetCacheKey(tenantId, editionId, editionPaymentType, paymentPeriodType);
            _cacheManager.GetCache(CacheName).Remove(key);
        }

        private static string GetCacheKey(int tenantId,
          int editionId,
          EditionPaymentType editionPaymentType,
          PaymentPeriodType? paymentPeriodType)
        {
            if (paymentPeriodType.HasValue)
                return tenantId + "_" + editionId + "_" + editionPaymentType.ToString() + "_" + paymentPeriodType.ToString();
            else
                return tenantId + "_" + editionId + "_" + editionPaymentType.ToString();
        }
    }
}
