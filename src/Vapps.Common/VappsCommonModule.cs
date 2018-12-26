using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero;

namespace Vapps.Common
{
    [DependsOn(
    typeof(AbpZeroCommonModule),
    typeof(AbpZeroCoreModule))]
    public class VappsCommonModule : AbpModule
    {
        public override void PreInitialize()
        {
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsCommonModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
