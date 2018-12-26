using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Vapps.SMS;
using Vapps.Web.SMS.Providers.Alidayu;

namespace Vapps.Web.Startup
{
    public static class CommunicationConfigurer
    {
        /// <summary>
        /// Configures the specified application.
        /// </summary>
        /// <param name="app">The application.</param>
        /// <param name="configuration">The configuration.</param>
        public static void Configure(IApplicationBuilder app, IConfiguration configuration)
        {
            var smsConfiguration = app.ApplicationServices.GetRequiredService<SMSConfiguration>();
            if (bool.Parse(configuration["SMS:Alidayu:IsEnabled"]))
            {
                smsConfiguration.Providers.Add(new SMSProviderInfo(AlidayuProvider.Name,
                    configuration["SMS:Alidayu:AppId"],
                    configuration["SMS:Alidayu:AppSecret"],
                        typeof(AlidayuProvider)));
            }
        }
    }
}
