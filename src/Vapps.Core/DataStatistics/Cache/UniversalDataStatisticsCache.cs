using Abp;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using Abp.Runtime.Session;
using System;
using System.Threading.Tasks;

namespace Vapps.DataStatistics.Cache
{
    public class UniversalDataStatisticsCache : IUniversalDataStatisticsCache, ISingletonDependency,
         IEventHandler<EntityDeletedEventData<UniversalDataStatistics>>
    {
        private readonly IRepository<UniversalDataStatistics, long> _repository;
        private readonly IAbpSession _abpSession;
        private readonly ICacheManager _cacheManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IUniversalDataStatisticsManager _universalDataStatisticsManager;

        public UniversalDataStatisticsCache(IRepository<UniversalDataStatistics, long> repository,
            IAbpSession abpSession,
            ICacheManager cacheManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager,
            IUniversalDataStatisticsManager universalDataStatisticsManager)
        {
            _repository = repository;
            _abpSession = abpSession;
            _cacheManager = cacheManager;
            _unitOfWorkManager = unitOfWorkManager;
            _objectMapper = objectMapper;
            _universalDataStatisticsManager = universalDataStatisticsManager;
        }

        [UnitOfWork]
        public async Task<UniversalDataStatisticsCacheItem> GetAsync(DateTime date, UniversalDataType type)
        {
            var cacheItem = await GetOrNullAsync(date, type);
            //if (cacheItem == null)
            //{
            //    throw new AbpException($"There is no universal data statistics with given data:{date} type:{type}");
            //}

            return cacheItem;
        }

        [UnitOfWork]
        public virtual async Task<UniversalDataStatisticsCacheItem> GetOrNullAsync(DateTime date, UniversalDataType type)
        {
            using (_unitOfWorkManager.Current.SetTenantId(_abpSession.TenantId))
            {
                string cacheKey = $"{_abpSession.TenantId}-{date.ToString("d")}-{type.ToString()}";
                return await _cacheManager.GetUniversalDataStatisticsCache().GetAsync(cacheKey, async () =>
                 {
                     var statistics = await _repository.FirstOrDefaultAsync(uds => uds.Date == date && uds.DataType == type);
                     if (statistics == null)
                     {
                         return null;
                     }

                     return CreateUniversalDataStatisticsCacheItem(statistics);
                 });
            }
        }

        protected virtual UniversalDataStatisticsCacheItem CreateUniversalDataStatisticsCacheItem(UniversalDataStatistics universalDataStatistics)
        {
            var statisticsCache = _objectMapper.Map<UniversalDataStatisticsCacheItem>(universalDataStatistics);

            return statisticsCache;
        }

        public void HandleEvent(EntityDeletedEventData<UniversalDataStatistics> eventData)
        {
            string cacheKey = $"{eventData.Entity.TenantId}-{eventData.Entity.Date.ToString("d")}-{eventData.Entity.DataType.ToString()}";
            _cacheManager
                .GetUniversalDataStatisticsCache()
                .Remove(cacheKey);
        }
    }
}
