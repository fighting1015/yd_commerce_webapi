using Abp.Runtime.Caching;
using System.Collections.Generic;
using Vapps.Dto;

namespace Vapps.Caching
{
    public static class SelectListItemCacheItemExtensions
    {
        public static ITypedCache<string, List<SelectListItemDto<T>>> GetSelectListItemCache<T>(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, List<SelectListItemDto<T>>>(ApplicationCacheNames.SelectItem);
        }
    }
}
