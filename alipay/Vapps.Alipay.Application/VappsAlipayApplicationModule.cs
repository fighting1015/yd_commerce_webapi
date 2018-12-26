using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Vapps.Alipay.Infrastructure
{
    [DependsOn(
        typeof(VappsAlipayInfrastructureModule))]
    public class VappsAlipayApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {

        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsAlipayApplicationModule).GetAssembly());


        }

        public override void PostInitialize()
        {
            Configuration.Modules.AbpAspNetCore()
           .CreateControllersForAppServices(
               typeof(VappsAlipayApplicationModule).GetAssembly()
           );
        }
    }
}
