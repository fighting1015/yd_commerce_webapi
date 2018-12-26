using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using System.Collections.Generic;
using Vapps.Messages;

namespace Vapps.SMS.Cache
{
    public class SMSTemplateCache : ISMSTemplateCache, ISingletonDependency,
        IEventHandler<EntityChangedEventData<SMSTemplate>>,
        IEventHandler<EntityDeletedEventData<SMSTemplate>>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IRepository<SMSTemplate, long> _smsTemplateRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IMessageTokenProvider _messageTokenProvider;

        public SMSTemplateCache(
            ICacheManager cacheManager,
            IObjectMapper objectMapper,
            IRepository<SMSTemplate, long> smsTemplateRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IMessageTokenProvider messageTokenProvider)
        {
            _cacheManager = cacheManager;
            _smsTemplateRepository = smsTemplateRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _objectMapper = objectMapper;
            _messageTokenProvider = messageTokenProvider;
        }

        public SMSTemplateCacheItem Get(long id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no sms template with given id: " + id);
            }

            return cacheItem;
        }

        public SMSTemplateCacheItem GetOrNull(long id)
        {
            return _cacheManager
               .GetSMSTemplateCache()
               .Get(id, () =>
               {
                   var outlet = GetSMSTemplateOrNull(id);
                   if (outlet == null)
                   {
                       return null;
                   }

                   return CreateSMSTemplateCacheItem(outlet);
               });
        }

        protected virtual SMSTemplateCacheItem CreateSMSTemplateCacheItem(SMSTemplate smsTemplate)
        {
            var templateCache = _objectMapper.Map<SMSTemplateCacheItem>(smsTemplate);

            templateCache.Items = _objectMapper.Map<List<SMSTemplateItemCache>>(smsTemplate.Items);

            return templateCache;
        }

        [UnitOfWork]
        protected virtual SMSTemplate GetSMSTemplateOrNull(long id)
        {
            var template = _smsTemplateRepository.FirstOrDefault(id);

            _smsTemplateRepository.EnsureCollectionLoaded(template, t => t.Items);

            return template;
        }

        public void HandleEvent(EntityChangedEventData<SMSTemplate> eventData)
        {
            _cacheManager
                .GetSMSTemplateCache()
                .Remove(eventData.Entity.Id);
        }

        public void HandleEvent(EntityDeletedEventData<SMSTemplate> eventData)
        {
            _cacheManager
                .GetSMSTemplateCache()
                .Remove(eventData.Entity.Id);
        }
    }
}
