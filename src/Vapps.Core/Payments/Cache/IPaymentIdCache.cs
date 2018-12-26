using Vapps.Editions;

namespace Vapps.Payments.Cache
{
    public interface IPaymentIdCache
    {
        string GetCacheItem(int tenantId,
            int editionId,
            EditionPaymentType editionPaymentType,
            PaymentPeriodType? PaymentPeriodType);

        void RemoveCacheItem(int tenantId,
            int editionId,
            EditionPaymentType editionPaymentType,
            PaymentPeriodType? PaymentPeriodType);
    }
}
