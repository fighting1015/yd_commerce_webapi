using Abp.Modules;
using Abp.Reflection.Extensions;

namespace Vapps.WeChat.Core
{
    [DependsOn(
    typeof(VappsCoreModule))]
    public class VappsECommerceCoreModule : AbpModule
    {
        public override void PreInitialize()
        {
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsECommerceCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
        }
    }
}
