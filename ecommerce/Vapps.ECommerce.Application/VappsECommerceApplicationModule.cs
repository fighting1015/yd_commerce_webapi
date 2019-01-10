using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Vapps.ECommerce.Core;

namespace Vapps.ECommerce
{
    [DependsOn(
        typeof(VappsECommerceCoreModule))]
    public class VappsECommerceApplicationModule : AbpModule
    {
        /// <summary>
        /// 
        /// </summary>
        public override void PreInitialize()
        {
            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);
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
