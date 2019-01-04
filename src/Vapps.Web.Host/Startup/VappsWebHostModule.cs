using Abp.Configuration;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Threading.BackgroundWorkers;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Vapps.Configuration;
using Vapps.EntityFrameworkCore;
using Vapps.Extensions;
using Vapps.ExternalAuthentications;
using Vapps.MultiTenancy;
using Vapps.Url;
using Vapps.Web.Authentication.External;

namespace Vapps.Web.Startup
{
    [DependsOn(
        typeof(VappsWebCoreModule))]
    public class VappsWebHostModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public VappsWebHostModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            RegisterComponent();

            Configuration.Modules.AbpWebCommon().MultiTenancy.DomainFormat = _appConfiguration["App:ServerRootAddress"] ?? "http://localhost:6001/";
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsWebHostModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            using (var scope = IocManager.CreateScope())
            {
                if (!scope.Resolve<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    return;
                }
            }

            if (IocManager.Resolve<IMultiTenancyConfig>().IsEnabled)
            {
                var workManager = IocManager.Resolve<IBackgroundWorkerManager>();
                workManager.Add(IocManager.Resolve<SubscriptionExpirationCheckWorker>());
                workManager.Add(IocManager.Resolve<SubscriptionExpireEmailNotifierWorker>());
            }
            ConfigureExternalAuthProviders();
            //Configuration.Modules.AbpWebApi().HttpConfiguration.ormatters.JsonFormatter.SerializerSettings.ContractResolver = new DefaultContractResolver();
        }

        private void RegisterComponent()
        {
            IocManager.IocContainer.Register(
                  Component.For<IAppUrlService>().ImplementedBy<AngularAppUrlService>().LifestyleTransient()
              );

            // IocManager.IocContainer.Register(
            //    Component.For<IEmailSender>().ImplementedBy<MailKitEmailSender>().LifestyleTransient()
            //);
        }

        private void ConfigureExternalAuthProviders()
        {
            var externalAuthConfiguration = IocManager.Resolve<ExternalAuthConfiguration>();
            var settingManager = IocManager.Resolve<ISettingManager>();

            var externalAuthentications = GetExternalAuthenticationsAsync(settingManager);
            var subTypeQuery = ExternalAuthProviderHelper.GetAllExternalAuthProviderTypeInfo();

            foreach (var item in externalAuthentications)
            {
                if (!item.IsEnabled)
                    continue;

                var provider = subTypeQuery.FirstOrDefault(t => t.Name.StartsWith(item.ProviderName));
                if (provider == null)
                    continue;

                externalAuthConfiguration.Providers.Add(new ExternalLoginProviderInfo(item.ProviderName,
                    item.AppId,
                    item.AppSecret,
                    item.ShowOnLoginPage,
                    provider.GetTypeInfo().AsType()));
            }
        }

        /// <summary>
        /// 获取外部认证设置
        /// </summary>
        /// <returns></returns>
        private List<ExternalAuthenticationProvider> GetExternalAuthenticationsAsync(ISettingManager settingManager)
        {

            var externalAuthentications = new List<ExternalAuthenticationProvider>();
            var providers = _appConfiguration["Authentication:Provider"];

            if (!providers.IsNullOrEmpty())
            {
                foreach (var provider in providers.Split(','))
                {
                    var itemString = settingManager.GetSettingValueAsync(string.Format(AppSettings.ExternalAuthentication.ProviderName, provider)).Result;
                    var item = new ExternalAuthenticationProvider() { ProviderName = provider, IsEnabled = false };

                    if (!itemString.IsNullOrEmpty())
                    {
                        item = JsonConvert.DeserializeObject<ExternalAuthenticationProvider>(itemString);
                    }

                    externalAuthentications.Add(item);
                }
            }

            return externalAuthentications;
        }
    }
}
