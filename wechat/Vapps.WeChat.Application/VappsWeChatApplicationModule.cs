using Abp.AspNetCore.Configuration;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Vapps.WeChat.Core;

namespace Vapps.WeChat.Application
{
    [DependsOn(
        typeof(VappsWeChatCoreModule),
        typeof(VappsWeChatInfrastructureModule))]
    public class VappsWeChatApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
        
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsWeChatApplicationModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            Configuration.Modules.AbpAspNetCore()
            .CreateControllersForAppServices(
                typeof(VappsWeChatApplicationModule).GetAssembly()
            );
        }
    }
}
