using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Vapps.WeChat.Core;

namespace Vapps.WeChat.Application
{
    [DependsOn(
        typeof(VappsECommerceCoreModule))]
    public class VappsECommerceApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
        
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsECommerceApplicationModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            Configuration.Modules.AbpAspNetCore()
            .CreateControllersForAppServices(
                typeof(VappsECommerceApplicationModule).GetAssembly()
            );
        }
    }
}
