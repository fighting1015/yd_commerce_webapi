using Abp;
using Abp.AutoMapper;
using Abp.Configuration.Startup;
using Abp.Dependency;
using Abp.Modules;
using Abp.Net.Mail;
using Abp.TestBase;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Moq;
using NSubstitute;
using System;
using System.IO;
using Vapps.Authorization.Users;
using Vapps.Configuration;
using Vapps.ECommerce;
using Vapps.ECommerce.Products;
using Vapps.EntityFrameworkCore;
using Vapps.FileStorage;
using Vapps.MultiTenancy;
using Vapps.Security.Recaptcha;
using Vapps.Tests.Configuration;
using Vapps.Tests.DependencyInjection;
using Vapps.Tests.Url;
using Vapps.Tests.Web;
using Vapps.Url;
using Vapps.Web.Security.CaptchaValidator;

namespace Vapps.Tests
{
    [DependsOn(
        typeof(VappsApplicationModule),
        typeof(VappsECommerceApplicationModule),
        typeof(VappsFileStorageModule),
        typeof(VappsEntityFrameworkCoreModule),
        typeof(AbpTestBaseModule))]
    public class VappsTestModule : AbpModule
    {
        public VappsTestModule(VappsEntityFrameworkCoreModule xiaoyuyueEntityFrameworkCoreModule)
        {
            xiaoyuyueEntityFrameworkCoreModule.SkipDbContextRegistration = true;
        }

        public override void PreInitialize()
        {
            var configuration = GetConfiguration();

            Configuration.UnitOfWork.Timeout = TimeSpan.FromMinutes(30);
            Configuration.UnitOfWork.IsTransactional = false;

            //Disable static mapper usage since it breaks unit tests (see https://github.com/aspnetboilerplate/aspnetboilerplate/issues/2052)
            Configuration.Modules.AbpAutoMapper().UseStaticMapper = false;

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();

            RegisterFakeService<AbpZeroDbMigrator>();

            IocManager.Register<IAppUrlService, FakeAppUrlService>();
            IocManager.Register<IWebUrlService, FakeWebUrlService>();
            IocManager.Register<ICaptchaValidator, FakeRecaptchaValidator>();

            IocManager.Register<IProductAppService, ProductAppService>();
            IocManager.Register<IProductAttributeAppService, ProductAttributeAppService>();

            //MockHostingEnvironment();

            var mockHttpContextAccessor = new Mock<IHttpContextAccessor>();

            IocManager.IocContainer.Register(
            Component.For<IHttpContextAccessor>().Instance(mockHttpContextAccessor.Object).LifestyleSingleton());

            IocManager.Register<ICaptchaValidator, LuosimaoCaptchaValidator>();

            Configuration.ReplaceService<IAppConfigurationAccessor, TestAppConfigurationAccessor>();
            Configuration.ReplaceService<IEmailSender, NullEmailSender>(DependencyLifeStyle.Transient);

            //Configuration.Modules.AspNetZero().LicenseCode = configuration["AbpZeroLicenseCode"];

            //Uncomment below line to write change logs for the entities below:
            Configuration.EntityHistory.IsEnabled = true;
            Configuration.EntityHistory.Selectors.Add("AbpZeroTemplateEntities", typeof(User), typeof(Tenant));

        }

        private void MockHostingEnvironment()
        {
            var env = new Mock<IHostingEnvironment>();

            var basePath = Directory.GetCurrentDirectory();

            if (basePath.Contains("test"))
            {
                basePath = basePath.Substring(0, basePath.IndexOf(@"\test\Vapps.Tests\bin\Debug\netcoreapp1.1"));
            }

            env.Setup(m => m.ContentRootPath).Returns(basePath + "\\src\\Vapps.Web.Host");
            env.Setup(m => m.EnvironmentName).Returns("Development");

            var mockEnvironmentObject = env.Object;

            IocManager.IocContainer.Register(
                  Component.For<IHostingEnvironment>().Instance(mockEnvironmentObject).LifestyleSingleton()
               );
        }

        public override void Initialize()
        {
            ServiceCollectionRegistrar.Register(IocManager);
        }

        private void RegisterFakeService<TService>()
            where TService : class
        {
            IocManager.IocContainer.Register(
                Component.For<TService>()
                    .UsingFactoryMethod(() => Substitute.For<TService>())
                    .LifestyleSingleton()
            );

        }

        private static IConfigurationRoot GetConfiguration()
        {
            return AppConfigurations.Get(Directory.GetCurrentDirectory(), addUserSecrets: true);
        }
    }
}
