using Abp.Runtime.Caching;
using Vapps.Helpers;

namespace Vapps.AccessRecords.Cache
{
    public static class IpAddressCacheExtension
    {
        public static ITypedCache<string, AddressData> GetIpAddressCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, AddressData>(ApplicationCacheNames.IpAddress);
        }
    }
}
