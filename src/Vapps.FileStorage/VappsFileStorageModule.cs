using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Zero;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Vapps.Common;
using Vapps.Extensions;
using Vapps.Providers;

namespace Vapps.FileStorage
{
    [DependsOn(
    typeof(VappsCommonModule),
    typeof(AbpZeroCoreModule))]
    public class VappsFileStorageModule : AbpModule
    {
        private readonly bool USE_HTTPS = false;

        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public VappsFileStorageModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = _env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            RegisterComponent();
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsFileStorageModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            var ak = _appConfiguration["FileStorage:Qiniu:AK"];

            Qiniu.Common.Config.AutoZone(ak, FileStorageConsts.IMAGE_BUCKET, USE_HTTPS);
        }

        private void RegisterComponent()
        {
            IocManager.IocContainer.Register(
                  Component.For<IStorageProvider>().ImplementedBy<QiniuStorageProvider>().LifestyleTransient()
              );
        }


    }
}
