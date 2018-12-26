using Abp.Dependency;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using Vapps.SMS;
using Vapps.States;

namespace Vapps.Caching
{
    /// <summary>
    /// 下拉框缓存清理类
    /// </summary>
    public class SelectListItemCacheSyncronizer :
       IEventHandler<EntityCreatedEventData<SMSTemplate>>,
       IEventHandler<EntityDeletedEventData<SMSTemplate>>,
       IEventHandler<EntityUpdatedEventData<SMSTemplate>>,

       IEventHandler<EntityCreatedEventData<Province>>,
       IEventHandler<EntityDeletedEventData<Province>>,
       IEventHandler<EntityUpdatedEventData<Province>>,

       IEventHandler<EntityCreatedEventData<City>>,
       IEventHandler<EntityDeletedEventData<City>>,
       IEventHandler<EntityUpdatedEventData<City>>,

       IEventHandler<EntityCreatedEventData<District>>,
       IEventHandler<EntityDeletedEventData<District>>,
       IEventHandler<EntityUpdatedEventData<District>>,

       ITransientDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IAbpSession _abpSession;

        public SelectListItemCacheSyncronizer(
            ICacheManager cacheManager,
            IObjectMapper objectMapper,
            IAbpSession abpSession)
        {
            _cacheManager = cacheManager;
            _objectMapper = objectMapper;
            _abpSession = abpSession;
        }

        public void HandleEvent(EntityCreatedEventData<SMSTemplate> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(ApplicationCacheNames.AvailableSmsTemplate);
        }

        public void HandleEvent(EntityUpdatedEventData<SMSTemplate> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(ApplicationCacheNames.AvailableSmsTemplate);
        }

        public void HandleEvent(EntityDeletedEventData<SMSTemplate> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(ApplicationCacheNames.AvailableSmsTemplate);
        }

        public void HandleEvent(EntityCreatedEventData<Province> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(ApplicationCacheNames.AvailableProvince);
        }

        public void HandleEvent(EntityDeletedEventData<Province> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(ApplicationCacheNames.AvailableProvince);
        }

        public void HandleEvent(EntityUpdatedEventData<Province> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(ApplicationCacheNames.AvailableProvince);
        }

        public void HandleEvent(EntityCreatedEventData<City> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(string.Format(ApplicationCacheNames.AvailableCity, eventData.Entity.ProvinceId));
        }

        public void HandleEvent(EntityUpdatedEventData<City> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(string.Format(ApplicationCacheNames.AvailableCity, eventData.Entity.ProvinceId));
        }

        public void HandleEvent(EntityDeletedEventData<City> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(string.Format(ApplicationCacheNames.AvailableCity, eventData.Entity.ProvinceId));
        }

        public void HandleEvent(EntityCreatedEventData<District> eventData)
        {
            _cacheManager.GetSelectListItemCache().Remove(string.Format(ApplicationCacheNames.AvailableCity, eventData.Entity.CityId));
        }

        public void HandleEvent(EntityDeletedEventData<District> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(string.Format(ApplicationCacheNames.AvailableCity, eventData.Entity.CityId));
        }

        public void HandleEvent(EntityUpdatedEventData<District> eventData)
        {
            _cacheManager.GetSelectListItemCache().RemoveAsync(string.Format(ApplicationCacheNames.AvailableCity, eventData.Entity.CityId));
        }
    }
}
