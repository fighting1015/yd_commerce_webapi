using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Extensions;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;

namespace Vapps.States.Cache
{
    public class StateCache : IStateCache, ISingletonDependency,
        IEventHandler<EntityChangedEventData<Province>>,
        IEventHandler<EntityDeletedEventData<Province>>,
        IEventHandler<EntityChangedEventData<City>>,
        IEventHandler<EntityDeletedEventData<City>>,
        IEventHandler<EntityChangedEventData<District>>,
        IEventHandler<EntityDeletedEventData<District>>
    {

        private readonly IRepository<Province> _provinceRepository;
        private readonly IRepository<City> _cityRepository;
        private readonly IRepository<District> _districtRepository;

        private readonly IObjectMapper _objectMapper;
        private readonly ICacheManager _cacheManager;


        public StateCache(IRepository<Province> provinceRepository,
            IRepository<City> cityRepository,
            IRepository<District> districtRepository,
            IObjectMapper objectMapper,
            ICacheManager cacheManager)
        {
            this._provinceRepository = provinceRepository;
            this._cityRepository = cityRepository;
            this._districtRepository = districtRepository;
            this._objectMapper = objectMapper;
            this._cacheManager = cacheManager;
        }

        public virtual ProvinceCacheItem GetProvince(int id)
        {
            var cacheItem = GetProvinceOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no province with given id: " + id);
            }

            return cacheItem;
        }

        public virtual ProvinceCacheItem GetProvinceOrNull(int id)
        {
            return _cacheManager.GetProvinceCache().Get(id, () =>
            {
                var province = _provinceRepository.FirstOrDefault(id);
                if (province == null)
                {
                    return null;
                }

                return CreateCacheItem(province);
            });
        }

        public virtual ProvinceCacheItem GetProvinceByNameOrNull(string name)
        {
            if (name.IsNullOrWhiteSpace())
                return null;

            return _cacheManager.GetProvinceByNameCache().Get(name, () =>
            {
                var province = _provinceRepository.FirstOrDefault(p => p.Name == name);
                if (province == null)
                {
                    return null;
                }

                return CreateCacheItem(province);
            });
        }

        public virtual CityCacheItem GetCity(int id)
        {
            var cacheItem = GetCityOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no city with given id: " + id);
            }

            return cacheItem;
        }

        public virtual CityCacheItem GetCityOrNull(int id)
        {
            return _cacheManager.GetCityCache().Get(id, () =>
            {
                var city = _cityRepository.FirstOrDefault(id);
                if (city == null)
                {
                    return null;
                }

                return CreateCacheItem(city);
            });
        }

        public virtual CityCacheItem GetCityByNameOrNull(string name)
        {
            if (name.IsNullOrWhiteSpace())
                return null;

            return _cacheManager.GetCityByNameCache().Get(name, () =>
            {
                var city = _cityRepository.FirstOrDefault(c => c.Name == name);
                if (city == null)
                {
                    return null;
                }

                return CreateCacheItem(city);
            });

        }

        public virtual DistrictCacheItem GetDistrict(int id)
        {
            var cacheItem = GetDistrictOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no district with given id: " + id);
            }

            return cacheItem;
        }

        public virtual DistrictCacheItem GetDistrictOrNull(int id)
        {
            return _cacheManager.GetDistrictCache().Get(id, () =>
            {
                var district = _districtRepository.FirstOrDefault(id);
                if (district == null)
                {
                    return null;
                }

                return CreateCacheItem(district);
            });
        }

        public virtual DistrictCacheItem GetDistrictByNameOrNull(string name)
        {
            if (name.IsNullOrWhiteSpace())
                return null;

            return _cacheManager.GetDistrictByNameCache().Get(name, () =>
            {
                var district = _districtRepository.FirstOrDefault(d => d.Name == name);
                if (district == null)
                {
                    return null;
                }

                return CreateCacheItem(district);
            });
        }
        protected virtual ProvinceCacheItem CreateCacheItem(Province province)
        {
            return _objectMapper.Map<ProvinceCacheItem>(province);
        }

        protected virtual CityCacheItem CreateCacheItem(City city)
        {
            return _objectMapper.Map<CityCacheItem>(city);
        }

        protected virtual DistrictCacheItem CreateCacheItem(District district)
        {
            return _objectMapper.Map<DistrictCacheItem>(district);
        }

        public void HandleEvent(EntityChangedEventData<Province> eventData)
        {
            _cacheManager
                .GetProvinceCache()
                .Remove(eventData.Entity.Id);

            _cacheManager
               .GetProvinceByNameCache()
               .Remove(eventData.Entity.Name);
        }

        public void HandleEvent(EntityDeletedEventData<Province> eventData)
        {
            _cacheManager
              .GetProvinceCache()
              .Remove(eventData.Entity.Id);

            _cacheManager
               .GetProvinceByNameCache()
               .Remove(eventData.Entity.Name);
        }

        public void HandleEvent(EntityChangedEventData<City> eventData)
        {
            _cacheManager
                .GetCityCache()
                .Remove(eventData.Entity.Id);

            _cacheManager
               .GetCityByNameCache()
               .Remove(eventData.Entity.Name);
        }

        public void HandleEvent(EntityDeletedEventData<City> eventData)
        {
            _cacheManager
               .GetCityCache()
               .Remove(eventData.Entity.Id);

            _cacheManager
               .GetCityByNameCache()
               .Remove(eventData.Entity.Name);
        }

        public void HandleEvent(EntityChangedEventData<District> eventData)
        {
            _cacheManager
                .GetDistrictCache()
                .Remove(eventData.Entity.Id);

            _cacheManager
               .GetDistrictByNameCache()
               .Remove(eventData.Entity.Name);
        }

        public void HandleEvent(EntityDeletedEventData<District> eventData)
        {
            _cacheManager
               .GetDistrictCache()
               .Remove(eventData.Entity.Id);

            _cacheManager
               .GetDistrictByNameCache()
               .Remove(eventData.Entity.Name);
        }
    }
}
