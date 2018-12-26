using Abp.Runtime.Caching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.WeChat.Core.TemplateMessages.Cache
{
    public static class TemplateMessageCacheExtension
    {
        public static ITypedCache<int, TemplateMessageCacheItem> GetTemplateMessageCache(this ICacheManager cacheManager)
        {
            return cacheManager.GetCache<int, TemplateMessageCacheItem>(TemplateMessageCacheItem.CacheName);
        }
    }
}
