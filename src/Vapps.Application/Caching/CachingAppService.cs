using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Runtime.Caching;
using Vapps.Authorization;
using Vapps.Caching.Dto;

namespace Vapps.Caching
{
    [AbpAuthorize(AdminPermissions.System.HostMaintenance)]
    public class CachingAppService : VappsAppServiceBase, ICachingAppService
    {
        private readonly ICacheManager _cacheManager;

        public CachingAppService(ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        /// <summary>
        /// 获取所有缓存
        /// </summary>
        /// <returns></returns>
        public ListResultDto<CacheDto> GetAllCaches()
        {
            var caches = _cacheManager.GetAllCaches()
                                        .Select(cache => new CacheDto
                                        {
                                            Name = cache.Name
                                        })
                                        .ToList();

            return new ListResultDto<CacheDto>(caches);
        }

        /// <summary>
        /// 清理缓存
        /// </summary>
        /// <param name="input">缓存Id</param>
        /// <returns></returns>
        public async Task ClearCache(EntityDto<string> input)
        {
            var cache = _cacheManager.GetCache(input.Id);
            await cache.ClearAsync();
        }

        /// <summary>
        /// 清理所有缓存
        /// </summary>
        /// <returns></returns>
        public async Task ClearAllCaches()
        {
            var caches = _cacheManager.GetAllCaches();
            foreach (var cache in caches)
            {
                await cache.ClearAsync();
            }
        }
    }
}