using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Vapps.Advert
{
    [DependsOn(
        typeof(VappsAdvertCoreModule))]
    public class VappsAdvertApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

            Configuration.Modules.AbpAspNetCore()
               .CreateControllersForAppServices(
                    typeof(VappsAdvertApplicationModule).GetAssembly()
           );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsAdvertApplicationModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
