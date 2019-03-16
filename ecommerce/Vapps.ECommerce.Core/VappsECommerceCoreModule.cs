using Abp.Modules;
using Abp.Reflection.Extensions;
using Castle.MicroKernel.Registration;
using Vapps.ECommerce.Shippings.Tracking;

namespace Vapps.ECommerce.Core
{
    [DependsOn(
    typeof(VappsCoreModule))]
    public class VappsECommerceCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            RegisterComponent();
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsECommerceCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }

        private void RegisterComponent()
        {
            IocManager.IocContainer.Register(
                  Component.For<IShipmentTracker>().ImplementedBy<ShipmentTracker>().LifestyleTransient()
              );
        }
    }
}
