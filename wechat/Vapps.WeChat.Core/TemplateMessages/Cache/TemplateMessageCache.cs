using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;

namespace Vapps.WeChat.Core.TemplateMessages.Cache
{
    public class TemplateMessageCache : ITemplateMessageCache, ISingletonDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IRepository<TemplateMessage> _templateMessageRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public TemplateMessageCache(
            ICacheManager cacheManager,
            IRepository<TemplateMessage> templateMessageRepository,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _templateMessageRepository = templateMessageRepository;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public virtual TemplateMessageCacheItem Get(int id)
        {
            var cacheItem = GetOrNull(id);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tempalte message with given id: " + id);
            }

            return cacheItem;
        }

        public virtual TemplateMessageCacheItem GetOrNull(int id)
        {
            return _cacheManager.GetTemplateMessageCache().Get(id, () =>
            {
                var tempalteMessage = GetTempalteMessgaeOrNull(id);
                if (tempalteMessage == null)
                {
                    return null;
                }
                return CreateCacheItem(tempalteMessage);
            });
        }

        protected virtual TemplateMessageCacheItem CreateCacheItem(TemplateMessage tempalte)
        {
            _templateMessageRepository.EnsureCollectionLoaded(tempalte, t => t.MessageItems);

            var cacheItem = new TemplateMessageCacheItem
            {
                Id = tempalte.Id,
                Name = tempalte.Name,
                FirstData = tempalte.FirstData,
                FirstDataColor = tempalte.FirstDataColor,
                RemarkData = tempalte.RemarkData,
                RemarkDataColor = tempalte.RemarkDataColor,
                TemplateId = tempalte.TemplateId,
                TemplateIdShort = tempalte.TemplateIdShort,
                Url = tempalte.Url,
                IsActive = tempalte.IsActive
            };

            foreach (var item in tempalte.MessageItems)
            {
                cacheItem.Items.Add(new TemplateMessageItemCacheItem()
                {
                    Color = item.Color,
                    DataName = item.DataName,
                    DataValue = item.DataValue,
                    Id = item.Id,
                });
            }
            return cacheItem;
        }

        [UnitOfWork]
        protected virtual TemplateMessage GetTempalteMessgaeOrNull(int id)
        {
            return _templateMessageRepository.FirstOrDefault(id);
        }

    }
}
