using Abp.Domain.Entities.Caching;
using System.Threading.Tasks;

namespace Vapps.Authorization.Accounts.Cache
{
    public interface IAccountCache : IEntityCache<AccountCacheItem, long>
    {
       
    }
}
