using Abp;
using Abp.Application.Editions;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.MultiTenancy;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using Abp.Runtime.Security;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vapps.Authorization.Users;
using Vapps.Editions.Cache;
using Vapps.Media;

namespace Vapps.MultiTenancy
{
    public class VappsTenantCache : TenantCache<Tenant, User>,
        IEventHandler<EntityChangedEventData<Tenant>>,
        IEventHandler<EntityUpdatedEventData<Edition>>,
        IEventHandler<EntityDeletedEventData<Edition>>
    {
        private readonly ICacheManager _cacheManager;
        private readonly IPictureManager _pictureManager;
        private readonly IRepository<Tenant> _tenantRepository;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IObjectMapper _objectMapper;

        public VappsTenantCache(
            ICacheManager cacheManager,
            IPictureManager pictureManager,
            IRepository<Tenant> tenantRepository,
            IUnitOfWorkManager unitOfWorkManager,
            IObjectMapper objectMapper) : base(cacheManager, tenantRepository, unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _tenantRepository = tenantRepository;
            _unitOfWorkManager = unitOfWorkManager;
            _pictureManager = pictureManager;
            _objectMapper = objectMapper;
        }

        protected new VappsTenantCacheItem CreateTenantCacheItem(Tenant tenant)
        {
            _tenantRepository.EnsurePropertyLoaded(tenant, t => t.Edition);

            var cacheItem = new VappsTenantCacheItem
            {
                Id = tenant.Id,
                Name = tenant.Name,
                TenancyName = tenant.TenancyName,
                BackgroundPictureId = tenant.BackgroundPictureId,
                LogoId = tenant.LogoId,
                Description = tenant.Description,
                Tagline = tenant.Tagline,
                BackgroundPictureUrl = _pictureManager.GetPictureUrl(tenant.BackgroundPictureId),
                LogoUrl = _pictureManager.GetPictureUrl(tenant.LogoId),
                EditionId = tenant.EditionId,
                ConnectionString = SimpleStringCipher.Instance.Decrypt(tenant.ConnectionString),
                IsActive = tenant.IsActive,
                SubscriptionEndDateUtc = tenant.SubscriptionEndDateUtc,
                CreationTime = tenant.CreationTime,
                IsInTrialPeriod = tenant.IsInTrialPeriod,
                HadTrialed = tenant.HadTrialed,
            };

            cacheItem.Edition = _objectMapper.Map<SubscribableEditionCacheItem>(tenant.Edition);

            return cacheItem;
        }

        public new VappsTenantCacheItem Get(int tenantId)
        {
            var cacheItem = GetOrNull(tenantId);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given id: " + tenantId);
            }

            return cacheItem;
        }

        public new VappsTenantCacheItem Get(string tenancyName)
        {
            var cacheItem = GetOrNull(tenancyName);

            if (cacheItem == null)
            {
                throw new AbpException("There is no tenant with given tenancy name: " + tenancyName);
            }

            return cacheItem;
        }

        public new VappsTenantCacheItem GetOrNull(string tenancyName)
        {
            var tenantId = _cacheManager.GetTenantByNameCache().Get(tenancyName.ToLowerInvariant(),
                () => GetTenantOrNull(tenancyName)?.Id);

            if (tenantId == null)
            {
                return null;
            }

            return Get(tenantId.Value);
        }

        public new VappsTenantCacheItem GetOrNull(int tenantId)
        {
            return _cacheManager.GetTenantCache().Get(tenantId, () =>
                   {
                       var tenant = GetTenantOrNull(tenantId);
                       if (tenant == null)
                       {
                           return null;
                       }

                       return CreateTenantCacheItem(tenant);
                   }
                );
        }

        public new void HandleEvent(EntityChangedEventData<Tenant> eventData)
        {
            var existingCacheItem = _cacheManager.GetTenantCache().GetOrDefault(eventData.Entity.Id);

            _cacheManager
                .GetTenantByNameCache()
                .Remove(
                    existingCacheItem != null
                        ? existingCacheItem.TenancyName.ToLowerInvariant()
                        : eventData.Entity.TenancyName.ToLowerInvariant()
                );

            _cacheManager
                .GetTenantCache()
                .Remove(eventData.Entity.Id);

            base.HandleEvent(eventData);
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityUpdatedEventData<Edition> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var tenants = _tenantRepository.GetAll().AsNoTracking().Where(t => t.EditionId == eventData.Entity.Id);
                foreach (var tenant in tenants)
                {
                    _cacheManager.GetTenantByNameCache().Remove(tenant.TenancyName);

                    _cacheManager.GetTenantCache().Remove(tenant.Id);
                }
            }
        }

        [UnitOfWork]
        public virtual void HandleEvent(EntityDeletedEventData<Edition> eventData)
        {
            using (_unitOfWorkManager.Current.SetTenantId(null))
            {
                var tenants = _tenantRepository.GetAll().AsNoTracking().Where(t => t.EditionId == eventData.Entity.Id);
                foreach (var tenant in tenants)
                {
                    _cacheManager.GetTenantByNameCache().Remove(tenant.TenancyName);

                    _cacheManager.GetTenantCache().Remove(tenant.Id);
                }
            }
        }
    }
}
