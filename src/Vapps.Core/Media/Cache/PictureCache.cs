using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using System.Threading.Tasks;

namespace Vapps.Media.Cache
{
    public class PictureCache : IPictureCache, ISingletonDependency,
         IEventHandler<EntityDeletedEventData<Picture>>,
         IEventHandler<EntityChangedEventData<Picture>>
    {
        private readonly IRepository<Picture, long> _pictureRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly ICacheManager _cacheManager;
        private readonly IObjectMapper _objectMapper;

        public PictureCache(IRepository<Picture, long> pictureRepository,
            IUnitOfWorkManager unitOfWorkManager,
            ICacheManager cacheManager,
            IObjectMapper objectMapper)
        {
            this._pictureRepository = pictureRepository;
            this._unitOfWorkManager = unitOfWorkManager;
            this._cacheManager = cacheManager;
            this._objectMapper = objectMapper;
        }

        public PictureCacheItem Get(long id)
        {
            var cacheItem = GetOrNull(id);

            return cacheItem;
        }

        public async Task<PictureCacheItem> GetAsync(long id)
        {
            var cacheItem = await GetOrNullAsync(id);

            return cacheItem;
        }

        public PictureCacheItem GetOrNull(long id)
        {
            return _cacheManager.GetPictureCache().Get(id, () =>
            {
                var picture = GetPictureOrNull(id);
                if (picture == null)
                {
                    return null;
                }

                return CreatePictureCacheItem(picture);
            });
        }

        public async Task<PictureCacheItem> GetOrNullAsync(long id)
        {
            return await _cacheManager.GetPictureCache().GetAsync(id, async () =>
              {
                  var picture = await GetPictureOrNullAsync(id);
                  if (picture == null)
                  {
                      return null;
                  }

                  return CreatePictureCacheItem(picture);
              });
        }

        protected virtual PictureCacheItem CreatePictureCacheItem(Picture picture)
        {
            var pictureCacheItem = _objectMapper.Map<PictureCacheItem>(picture);

            return pictureCacheItem;
        }

        [UnitOfWork]
        protected virtual Picture GetPictureOrNull(long id)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var picture = _pictureRepository.FirstOrDefault(id);

                return picture;
            }
        }

        [UnitOfWork]
        protected virtual async Task<Picture> GetPictureOrNullAsync(long id)
        {
            using (_unitOfWorkManager.Current.DisableFilter(AbpDataFilters.MayHaveTenant))
            {
                var picture = await _pictureRepository.FirstOrDefaultAsync(id);

                return picture;
            }
        }

        public void HandleEvent(EntityDeletedEventData<Picture> eventData)
        {
            _cacheManager.GetPictureCache().Remove(eventData.Entity.Id);
        }

        public void HandleEvent(EntityChangedEventData<Picture> eventData)
        {
            _cacheManager.GetPictureCache().Remove(eventData.Entity.Id);
        }
    }
}
