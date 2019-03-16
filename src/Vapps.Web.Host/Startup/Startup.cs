using Abp.AspNetCore;
using Abp.Castle.Logging.Log4Net;
using Abp.Extensions;
using Abp.PlugIns;
using Abp.Timing;
using Castle.Facilities.Logging;
using Hangfire;
using Hangfire.MySql.Core;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Cors.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.PlatformAbstractions;
using Swashbuckle.AspNetCore.Swagger;
using System;
using System.IO;
using System.Linq;
using Vapps.Alipay.Infrastructure;
using Vapps.Common;
using Vapps.Debugging;
using Vapps.ECommerce.Orders.Jobs;
using Vapps.EntityFrameworkCore;
using Vapps.Extensions;
using Vapps.Identity;
using Vapps.Web.Authentication.JwtBearer;
using Vapps.Web.Common;
using Vapps.Web.Hangfire;
using Vapps.Web.IdentityServer;

using ILogger = Microsoft.Extensions.Logging.ILogger;
using ILoggerFactory = Microsoft.Extensions.Logging.ILoggerFactory;


namespace Vapps.Web.Startup
{
    public class Startup
    {
        private const string DefaultCorsPolicyName = "localhost";

        private readonly IConfigurationRoot _appConfiguration;
        private readonly IHostingEnvironment _hostingEnvironment;

        public Startup(IHostingEnvironment env)
        {
            _hostingEnvironment = env;
            _appConfiguration = env.GetAppConfiguration();
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            Clock.Provider = ClockProviders.Utc;

            //MVC
            services.AddMvc(options =>
            {
                options.Filters.Add(new CorsAuthorizationFilterFactory(DefaultCorsPolicyName));
            }).SetCompatibilityVersion(CompatibilityVersion.Version_2_1); ;

            //Configure CORS for angular2 UI
            services.AddCors(options =>
            {
                options.AddPolicy(DefaultCorsPolicyName, builder =>
                {
                    //App:CorsOrigins in appsettings.json can contain more than one address with splitted by comma.
                    builder
                        .WithOrigins(
                            // App:CorsOrigins in appsettings.json can contain more than one address separated by comma.
                            _appConfiguration["App:CorsOrigins"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray()
                        )
                        .SetIsOriginAllowedToAllowWildcardSubdomains()
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            IdentityRegistrar.Register(services);
            AlipayRegistrar.Register(services, _appConfiguration);
            AuthConfigurer.Configure(services, _appConfiguration);

            //Identity server
            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                IdentityServerRegistrar.Register(services, _appConfiguration);
            }

            if (bool.Parse(_appConfiguration["AppSettings:EnableSwagger"]))
            {
                //Swagger - Enable this line and the related lines in Configure method to enable swagger UI
                services.AddSwaggerGen(options =>
                {
                    ConfigureSwaggerUi(options);
                });
            }

            //Hangfire (Enable to use Hangfire instead of default job manager)
            services.AddHangfire(config =>
            {
                //config.UseStorage(new MySqlStorage(_appConfiguration.GetConnectionString("Hangfire")));
                config.UseSqlServerStorage(_appConfiguration.GetConnectionString("Hangfire"));

                //var options = new RedisStorageOptions();
                //options.Db = _appConfiguration.GetValue<int>("Hangfire:DatabaseId");
                //config.UseRedisStorage(_appConfiguration.GetConnectionString("Hangfire"), options);
            });

            //Configure Abp and Dependency Injection
            return services.AddAbp<VappsWebHostModule>(options =>
            {
                //Configure Log4Net logging
                options.IocManager.IocContainer.AddFacility<LoggingFacility>(
                    f => f.UseAbpLog4Net().WithConfig("log4net.config")
                );

                options.PlugInSources.AddFolder(Path.Combine(_hostingEnvironment.WebRootPath, "Plugins"), SearchOption.AllDirectories);
            });
        }

        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory)
        {
            //Initializes ABP framework.
            app.UseAbp(options =>
            {
                options.UseAbpRequestLocalization = false; //used below: UseAbpRequestLocalization
            });

            app.UseCors(DefaultCorsPolicyName); //Enable CORS!

            app.UseAuthentication();
            app.UseJwtTokenMiddleware();

            if (bool.Parse(_appConfiguration["IdentityServer:IsEnabled"]))
            {
                app.UseJwtTokenMiddleware("IdentityBearer");
                app.UseIdentityServer();
            }

            CommunicationConfigurer.Configure(app, _appConfiguration);

            app.UseStaticFiles();

            using (var scope = app.ApplicationServices.CreateScope())
            {
                if (scope.ServiceProvider.GetService<DatabaseCheckHelper>().Exist(_appConfiguration["ConnectionStrings:Default"]))
                {
                    app.UseAbpRequestLocalization();
                }
            }

            //Hangfire dashboard & server (Enable to use Hangfire instead of default job manager)
            app.UseHangfireDashboard("/hangfire", new DashboardOptions
            {
                //Authorization = new[] { new AbpHangfireAuthorizationFilter(AdminPermissions.UserManage.Users.Create) }
            });

            var jobOptions = new BackgroundJobServerOptions
            {
                Queues = _appConfiguration["Hangfire:Queues"]
                                .Split(",", StringSplitOptions.RemoveEmptyEntries)
                                .Select(o => o.RemovePostFix("/"))
                                .ToArray(),
                WorkerCount = _appConfiguration.GetValue<int>("Hangfire:WorkerCount"), // 并发任务数
                ServerName = _appConfiguration["Hangfire:ServerName"],// 服务器名称
            };
            app.UseHangfireServer(jobOptions);

            app.UseHangfireServer();
            StratHangfireMission();

            app.UseMvc(routes =>
            {
                routes.MapRoute(
                    name: "defaultWithArea",
                    template: "{area}/{controller=Home}/{action=Index}/{id?}");

                routes.MapRoute(
                    name: "default",
                    template: "{controller=Home}/{action=Index}/{id?}");
            });


            if (bool.Parse(_appConfiguration["AppSettings:EnableSwagger"]))
            {
                // Enable middleware to serve generated Swagger as a JSON endpoint
                app.UseSwagger();
                // Enable middleware to serve swagger-ui assets (HTML, JS, CSS etc.)
                app.UseHSwaggerUI(options =>
                {
                    ConfigureHSwaggerUI(options);
                });
            }
        }

        /// <summary>
        /// 配置SwaggerUi
        /// </summary>
        /// <param name="options"></param>
        private static void ConfigureSwaggerUi(Swashbuckle.AspNetCore.SwaggerGen.SwaggerGenOptions options)
        {
            options.SwaggerDoc("v1", new Info { Title = "Vapps API", Version = "v1" });
            //options.DescribeAllEnumsAsStrings();
            options.DocInclusionPredicate((docName, description) => true);
            options.DocumentFilter<EnumDocumentFilter>();
            //Note: This is just for showing Authorize button on the UI. 
            //Authorize button's behaviour is handled in wwwroot/swagger/ui/index.html
            options.AddSecurityDefinition("Bearer", new BasicAuthScheme());

            var basePath = PlatformServices.Default.Application.ApplicationBasePath;

            options.IncludeXmlComments(basePath + "\\Vapps.Core.xml");
            options.IncludeXmlComments(basePath + "\\Vapps.Application.xml");
            options.IncludeXmlComments(basePath + "\\Vapps.Web.Core.xml");
            options.IncludeXmlComments(basePath + "\\Vapps.WeChat.Application.xml");
            options.IncludeXmlComments(basePath + "\\Vapps.Alipay.Application.xml");
            options.IncludeXmlComments(basePath + "\\Vapps.ECommerce.Application.xml");
        }

        private void StratHangfireMission()
        {
            if (!bool.Parse(_appConfiguration["Hangfire:IsEnabled"]))
                return;

            HangfireBackgroundJobManagerExtension.MinutelyRecurring<OrderSyncJob, int>(5, "order");

            if (!DebugHelper.IsDebug)
            {
                HangfireBackgroundJobManagerExtension.MinutelyRecurring<KeepAliveJob, bool>(true);
            }
        }

        /// <summary>
        /// 配置SwaggerUi
        /// </summary>
        /// <param name="options"></param>
        private static void ConfigureHSwaggerUI(Hamazon.AspNetCore.SwaggerUI.Application.SwaggerUIOptions options)
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "Vapps API V1");
            options.InjectOnCompleteJavaScript("/swagger/lang/zh-cn.js");  //语言文件
            options.InjectOnCompleteJavaScript("/swagger/js/translator.js"); //语言切换
        }
    }
}
