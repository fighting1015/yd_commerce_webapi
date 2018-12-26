using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using System;
using Vapps.Authorization;
using Vapps.DataStatistics.Cache;
using Vapps.FileStorage;
using Vapps.WeChat.Core;

namespace Vapps
{
    /// <summary>
    /// Application layer module of the application.
    /// </summary>
    [DependsOn(
        typeof(VappsFileStorageModule),
        typeof(VappsCoreModule))]
    public class VappsApplicationModule : AbpModule
    {
        /// <summary>
        /// 
        /// </summary>
        public override void PreInitialize()
        {
            //Adding authorization providers
            Configuration.Authorization.Providers.Add<AppAuthorizationProvider>();

            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

            ConfigCacheExpireTime();
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsApplicationModule).GetAssembly());
        }

        /// <summary>
        /// 缓存过期时间设置
        /// </summary>
        private void ConfigCacheExpireTime()
        {
            //Configuration for a specific cache
            Configuration.Caching.Configure(DataStatisticsCacheConst.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });
        }
    }
}