using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Vapps.Alipay.Infrastructure
{
    [DependsOn(
        typeof(VappsCoreModule))]
    public class VappsAlipayInfrastructureModule : AbpModule
    {
        public override void PreInitialize()
        {

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsAlipayInfrastructureModule).GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}
