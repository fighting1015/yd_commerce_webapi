using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Vapps.Advert.AdvertAccounts;
using Vapps.Advert.AdvertAccounts.Sync;
using Vapps.Advert.Configuration;

namespace Vapps.Advert
{
    [DependsOn(
    typeof(VappsCoreModule))]
    public class VappsAdvertCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding setting providers
            Configuration.Settings.Providers.Add<AdvertSettingProvider>();

            RegisterComponent();
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsAdvertCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }

        private void RegisterComponent()
        {
            IocManager.IocContainer.Register(
                 Component.For<IAdvertAccountSyncor>().ImplementedBy<TenantAdvertAccountSyncor>().LifestyleTransient().Named("TenantAdvertAccountSyncor")
             );

            IocManager.IocContainer.Register(
                Component.For<IAdvertAccountSyncor>().ImplementedBy<ToutiaoAdvertAccountSyncor>().LifestyleTransient().Named("ToutiaoAdvertAccountSyncor")
             );
        }
    }
}
