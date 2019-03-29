using Abp.AspNetCore.Configuration;
using Abp.AutoMapper;
using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Vapps.Statistic
{
    [DependsOn(
        typeof(VappsStatisticCoreModule))]
    public class VappsStatisticApplicationModule : AbpModule
    {
        public override void PreInitialize()
        {
            //Adding custom AutoMapper configuration
            Configuration.Modules.AbpAutoMapper().Configurators.Add(CustomDtoMapper.CreateMappings);

            Configuration.Modules.AbpAspNetCore()
               .CreateControllersForAppServices(
                    typeof(VappsStatisticCoreModule).GetAssembly()
           );
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsStatisticCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {

        }
    }
}
