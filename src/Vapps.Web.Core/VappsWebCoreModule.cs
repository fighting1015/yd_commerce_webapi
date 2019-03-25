using Abp.AspNetCore;
using Abp.AspNetCore.Configuration;
using Abp.Configuration.Startup;
using Abp.Hangfire;
using Abp.Hangfire.Configuration;
using Abp.IO;
using Abp.Modules;
using Abp.Reflection.Extensions;
using Abp.Runtime.Caching.Redis;
using Abp.Zero.Configuration;
using Castle.MicroKernel.Registration;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IO;
using System.Text;
using Vapps.Advert;
using Vapps.Alipay.Infrastructure;
using Vapps.Configuration;
using Vapps.ECommerce;
using Vapps.EntityFrameworkCore;
using Vapps.Extensions;
using Vapps.Security.Recaptcha;
using Vapps.Web.Authentication.JwtBearer;
using Vapps.Web.Authentication.TwoFactor;
using Vapps.Web.Configuration;
using Vapps.Web.Security.CaptchaValidator;
using Vapps.WeChat.Application;
namespace Vapps.Web
{
    [DependsOn(
        typeof(AbpAspNetCoreModule),
        typeof(AbpRedisCacheModule), //AbpRedisCacheModule dependency (and Abp.RedisCache nuget package) can be removed if not using Redis cache
        typeof(AbpHangfireAspNetCoreModule), //AbpHangfireModule dependency (and Abp.Hangfire.AspNetCore nuget package) can be removed if not using Hangfire
        typeof(VappsApplicationModule),
        typeof(VappsAlipayApplicationModule),
        typeof(VappsWeChatApplicationModule),
        typeof(VappsECommerceApplicationModule),
        typeof(VappsAdvertApplicationModule),
        typeof(VappsEntityFrameworkCoreModule)
    )]
    public class VappsWebCoreModule : AbpModule
    {
        private readonly IHostingEnvironment _env;
        private readonly IConfigurationRoot _appConfiguration;

        public VappsWebCoreModule(IHostingEnvironment env)
        {
            _env = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public override void PreInitialize()
        {
            //Set default connection string
            RegisterComponent();

            Configuration.DefaultNameOrConnectionString = _appConfiguration.GetConnectionString(
                VappsConsts.ConnectionStringName
            );

            //Use database for language management
            Configuration.Modules.Zero().LanguageManagement.EnableDbLocalization();
            Configuration.Modules.AbpAspNetCore()
                .CreateControllersForAppServices(
                    typeof(VappsApplicationModule).GetAssembly()
                );

            if (_appConfiguration["Authentication:JwtBearer:IsEnabled"] != null && bool.Parse(_appConfiguration["Authentication:JwtBearer:IsEnabled"]))
            {
                ConfigureTokenAuth();
            }

            Configuration.ReplaceService<IAppConfigurationAccessor, AppConfigurationAccessor>();

            //Uncomment this line to use Hangfire instead of default background job manager (remember also to uncomment related lines in Startup.cs file(s)).
            Configuration.BackgroundJobs.UseHangfire();

            //Uncomment this line to use Redis cache instead of in-memory cache.
            //See app.config for Redis configuration and connection string
            if (_appConfiguration["Abp:RedisCache:IsEnabled"] != null && bool.Parse(_appConfiguration["Abp:RedisCache:IsEnabled"]))
            {
                Configuration.Caching.UseRedis(options =>
                {
                    options.ConnectionString = _appConfiguration["Abp:RedisCache:ConnectionString"];
                    options.DatabaseId = _appConfiguration.GetValue<int>("Abp:RedisCache:DatabaseId");
                });
            }

            Configuration.Caching.Configure(AuthenticateResultCacheItem.CacheName, cache =>
            {
                cache.DefaultSlidingExpireTime = TimeSpan.FromMinutes(5);
            });

            Configuration.Caching.Configure(TwoFactorCodeCacheItem.CacheName, cache =>
            {
                cache.DefaultAbsoluteExpireTime = TimeSpan.FromMinutes(2);
            });
        }

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(typeof(VappsWebCoreModule).GetAssembly());
        }

        public override void PostInitialize()
        {
            SetAppFolders();
        }

        private void RegisterComponent()
        {
            IocManager.IocContainer.Register(
                  Component.For<ICaptchaValidator>().ImplementedBy<LuosimaoCaptchaValidator>().LifestyleTransient()
              );
        }

        private void ConfigureTokenAuth()
        {
            IocManager.Register<TokenAuthConfiguration>();
            var tokenAuthConfig = IocManager.Resolve<TokenAuthConfiguration>();

            tokenAuthConfig.SecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_appConfiguration["Authentication:JwtBearer:SecurityKey"]));
            tokenAuthConfig.Issuer = _appConfiguration["Authentication:JwtBearer:Issuer"];
            tokenAuthConfig.Audience = _appConfiguration["Authentication:JwtBearer:Audience"];
            tokenAuthConfig.SigningCredentials = new SigningCredentials(tokenAuthConfig.SecurityKey, SecurityAlgorithms.HmacSha256);
            tokenAuthConfig.Expiration = TimeSpan.FromDays(1);
        }

        private void SetAppFolders()
        {
            var appFolders = IocManager.Resolve<AppFolders>();

            appFolders.SampleProfileImagesFolder = Path.Combine(_env.WebRootPath, $"Common{Path.DirectorySeparatorChar}Images{Path.DirectorySeparatorChar}SampleProfilePics");
            appFolders.WebLogsFolder = Path.Combine(_env.ContentRootPath, $"App_Data{Path.DirectorySeparatorChar}Logs");

#if NET461
            if (_env.IsDevelopment())
            {
                var currentAssemblyDirectoryPath = typeof(VappsWebCoreModule).GetAssembly().GetDirectoryPathOrNull();
                if (currentAssemblyDirectoryPath != null)
                {
                    appFolders.WebLogsFolder = Path.Combine(currentAssemblyDirectoryPath, $"App_Data{Path.DirectorySeparatorChar}Logs");
                }
            }
#endif
        }
    }
}
