using System.Threading.Tasks;

namespace Vapps.Payments.Cache
{
    public interface ISubscriptionPaymentCache
    {
        Task<SubscriptionPaymentCacheItem> GetCacheItemOrNullAsync(string paymentId);

        Task AddCacheItemAsync(SubscriptionPaymentCacheItem item);

        Task RemoveCacheItemAsync(string paymentId);
    }
}
