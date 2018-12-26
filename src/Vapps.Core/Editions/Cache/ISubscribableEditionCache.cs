using System.Threading.Tasks;

namespace Vapps.Editions.Cache
{
    public interface ISubscribableEditionCache
    {
        Task<SubscribableEditionCacheItem> GetAsync(int id);

        Task<SubscribableEditionCacheItem> GetDefaultAsync();

        Task<SubscribableEditionCacheItem> GetHighestAsync();

        Task<SubscribableEditionCacheItem> GetOrNullAsync(int id);
    }
}
