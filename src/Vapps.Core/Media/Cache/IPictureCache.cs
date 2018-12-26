using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Media.Cache
{
    public interface IPictureCache
    {
        PictureCacheItem Get(long id);

        Task<PictureCacheItem> GetAsync(long id);

        PictureCacheItem GetOrNull(long id);

        Task<PictureCacheItem> GetOrNullAsync(long id);
    }
}
