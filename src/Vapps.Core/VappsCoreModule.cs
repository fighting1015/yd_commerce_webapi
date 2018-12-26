using Abp.Authorization.Users;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Events.Bus;
using Abp.Events.Bus.Entities;
using Abp.MailKit;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.Net.Mail.Smtp;
using Abp.Reflection.Extensions;
using Abp.Timing;
using Abp.Web.Models;
using Abp.Zero;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using System;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.Configuration;
using Vapps.DataStatistics.Cache;
using Vapps.Emailing;
using Vapps.ErrorInfos;
using Vapps.Features;
using Vapps.Localization;
using Vapps.MultiTenancy;
using Vapps.Payments.Cache;
using Vapps.Notifications;
using Vapps.Timing;

namespace Vapps
{
    [DependsOn(
     typeof(AbpZeroCommonModule),
     typeof(AbpZeroCoreModule),
     typeof(AbpAutoMapperModule),
     typeof(AbpMailKitModule))]
    public class VappsCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //workaround for issue: https://github.com/aspnet/EntityFrameworkCore/issues/9825
            //related github issue: https://github.com/aspnet/EntityFrameworkCore/issues/10407
            AppContext.SetSwitch("Microsoft.EntityFrameworkCore.Issue9825", true);

            Configuration.Auditing.IsEnabled = false;
            Configuration.Auditing.IsEnabledForAnonymousUsers = false;

            //Declare entity types
            Configuration.Modules.Zero().EntityTypes.Tenant = typeof(Tenant);
            Configuration.Modules.Zero().EntityTypes.Role = typeof(Role);
            Configuration.Modules.Zero().EntityTypes.User = typeof(User);

            VappsLocalizationConfigurer.Configure(Configuration.Localization);

            //Adding feature providers
            Configuration.Features.Providers.Add<AppFeatureProvider>();

            //Adding setting providers
            Configuration.Settings.Providers.Add<AppSettingProvider>();

            //Adding notification providers
            Configuration.Notifications.Providers.Add<AppNotificationProvider>();

            //Enable this line to create a multi-tenant application.
            Configuration.MultiTenancy.IsEnabled = VappsConsts.MultiTenancyEnabled;

            //Configure roles
            AppRoleConfig.Configure(Configuration.Modules.Zero().RoleManagement);

            //if (DebugHelper.IsDebug)
            //{
            //    //Disabling email sending in debug mode
            //    Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);
            //}

            Configuration.ReplaceService<IEmailSender, CustomEmailSender>(DependencyLifeStyle.Transient);

            Configuration.ReplaceService(typeof(IEmailSenderConfiguration), () =>
            {
                Configuration.IocManager.IocContainer.Register(
                    Component.For<IEmailSenderConfiguration, ISmtpEmailSenderConfiguration>()
                             .ImplementedBy<VappsSmtpEmailSenderConfiguration>()
                             .LifestyleTransient()
                );
            });

            ConfigCacheExpireTime();
        }

        /// <summary>
        /// 缓存过期时间设置
        /// </summary>
        private void ConfigCacheExpireTime()
        {
            //Configuration for a specific cache
            Configuration.Caching.Configure(ApplicationCacheNames.SelectItem, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });

            Configuration.Caching.Configure(ApplicationCacheNames.AvailableSmsTemplate, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });

            Configuration.Caching.Configure(ApplicationCacheNames.AvailableProvince, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });

            Configuration.Caching.Configure(ApplicationCacheNames.AvailableCity, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });

            Configuration.Caching.Configure(ApplicationCacheNames.AvailableDistrict, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });

            Configuration.Caching.Configure(PaymentIdCache.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(int.MaxValue);
            });

            Configuration.Caching.Configure(SubscriptionPaymentCacheItem.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(int.MaxValue);
            });

            Configuration.Caching.Configure(UniversalDataStatisticsCacheItem.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromDays(7);
            });
        }

        public override void Initialize()
        {
            var EventBus = IocManager.Resolve<IEventBus>();

            EventBus.UnregisterAll<EntityCreatedEventData<AbpUserBase>>();
            EventBus.UnregisterAll<EntityDeletedEventData<AbpUserBase>>();
            EventBus.UnregisterAll<EntityUpdatedEventData<AbpUserBase>>();
            IocManager.RegisterAssemblyByConvention(typeof(VappsCoreModule).GetAssembly());

            RegisterTenantCache();
            //RegisterErrorInfoBuilder();
        }

        public override void PostInitialize()
        {
            IocManager.Resolve<AppTimes>().StartupTime = Clock.Now;
        }

        private void RegisterTenantCache()
        {
            IocManager.IocContainer.Register(
                Component.For<VappsTenantCache>().ImplementedBy<VappsTenantCache>().LifestyleSingleton()
            );
        }

        private void RegisterErrorInfoBuilder()
        {
            IocManager.IocContainer.Register(
                Component.For<IErrorInfoBuilder>().ImplementedBy<VappsErrorInfoBuilder>().LifestyleSingleton()
            );
        }
    }
}