using Abp.Runtime.Caching;

namespace Vapps.Media.Cache
{
    public static class PictureCacheItemExtension
    {
        public static ITypedCache<long, PictureCacheItem> GetPictureCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<long, PictureCacheItem>(PictureCacheItem.CacheName);
        }
    }

}
