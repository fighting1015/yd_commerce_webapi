using Abp.Runtime.Caching;

namespace Vapps.SMS.Cache
{
    public static class SMSTemplateCacheItemExtensions
    {
        public static ITypedCache<long, SMSTemplateCacheItem> GetSMSTemplateCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<long, SMSTemplateCacheItem>(SMSTemplateCacheItem.CacheName);
        }
    }
}
