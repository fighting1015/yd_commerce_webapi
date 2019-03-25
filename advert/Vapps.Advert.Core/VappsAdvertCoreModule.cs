using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Vapps.Advert
{
    [DependsOn(
    typeof(VappsCoreModule))]
    public class VappsAdvertCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
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

        }
    }
}
