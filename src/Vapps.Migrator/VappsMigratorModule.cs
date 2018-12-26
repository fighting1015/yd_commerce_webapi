using Abp.Events.Bus;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Microsoft.Extensions.Configuration;
using Vapps.Configuration;
using Vapps.EntityFrameworkCore;
using Vapps.Migrator.DependencyInjection;

namespace Vapps.Migrator
{
    [DependsOn(typeof(VappsEntityFrameworkCoreModule))]
    public class VappsMigratorModule : AbpModule
    {
        private readonly IConfigurationRoot _appConfiguration;

        public VappsMigratorModule(VappsEntityFrameworkCoreModule xiaoyuyueZeroEntityFrameworkCoreModule)
        {
            xiaoyuyueZeroEntityFrameworkCoreModule.SkipDbSeed = true;

            _appConfiguration = AppConfigurations.Get(
                typeof(VappsMigratorModule).GetAssembly().GetDirectoryPathOrNull()
            );
        }

        public override void PreInitialize()
        {
            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                VappsConsts.ConnectionStringName
                );

            Configuration.BackgroundJobs.IsJobExecutionEnabled = false;
            Configuration.ReplaceService(typeof(IEventBus), () =>
            {
                IocManager.IocContainer.Register(
                    Component.For<IEventBus>().Instance(NullEventBus.Instance)
                );
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsMigratorModule).GetAssembly());
            ServiceCollectionRegistrar.Register(IocManager);
        }
    }
}