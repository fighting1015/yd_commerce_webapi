using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Vapps.Statistic
{
    [DependsOn(
    typeof(VappsCoreModule))]
    public class VappsStatisticCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
            RegisterComponent();
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsStatisticCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }

        private void RegisterComponent()
        {
           
        }
    }
}
