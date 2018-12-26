using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Events.Bus.Entities;
using Abp.Events.Bus.Handlers;
using Abp.Runtime.Caching;
using System;
using Vapps.AccessRecords;
using Vapps.DataStatistics.Dto;

namespace Vapps.DataStatistics.Cache
{
    public class DataStatisticsCacheSyncronizer :
        ITransientDependency
    {
        private readonly ICacheManager _cacheManager;

        /// <summary>
        /// Constructor
        /// </summary>
        public DataStatisticsCacheSyncronizer(
            ICacheManager cacheManager)
        {
            _cacheManager = cacheManager;
        }

        
    }
}
