using Abp.Runtime.Caching;
using System.Collections.Generic;
using Vapps.Common.Dto;

namespace Vapps.Caching
{
    public static class SelectListItemCacheItemExtensions
    {
        public static ITypedCache<string, List<SelectListItemDto>> GetSelectListItemCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<string, List<SelectListItemDto>>(ApplicationCacheNames.SelectItem);
        }
    }
}
