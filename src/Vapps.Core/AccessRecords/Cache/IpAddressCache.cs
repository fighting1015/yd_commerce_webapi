using Abp;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.ObjectMapping;
using Abp.Runtime.Caching;
using Abp.Threading;
using System;
using System.Threading.Tasks;
using Vapps.Helpers;

namespace Vapps.AccessRecords.Cache
{
    public class IpAddressCache : IIpAddressCache, ISingletonDependency
    {
        private readonly ICacheManager _cacheManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public IpAddressCache(
            ICacheManager cacheManager,
            IObjectMapper objectMapper,
            IUnitOfWorkManager unitOfWorkManager)
        {
            _cacheManager = cacheManager;
            _unitOfWorkManager = unitOfWorkManager;
            _objectMapper = objectMapper;
        }

        public AddressData Get(string ip)
        {
            var cacheItem = GetOrNull(ip);

            if (cacheItem == null)
            {
                throw new AbpException("There is no ip address with given ip: " + ip);
            }

            return cacheItem;
        }

        public AddressData GetOrNull(string ip)
        {
            return _cacheManager
               .GetIpAddressCache()
               .Get(ip, () =>
               {
                   var result = GetIpAddressOrNull(ip);
                   return result;
               });
        }

        protected virtual AddressData GetIpAddressOrNull(string ip)
        {
            return AsyncHelper.RunSync(async () =>
            {
                var addressData = await CommonHelper.AnalysisIp2AddressAsync(ip);
                return addressData?.AddressData ?? null;
            });
        }
    }
}
