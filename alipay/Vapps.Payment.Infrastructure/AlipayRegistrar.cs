using Alipay.AopSdk.AspnetCore;
using Alipay.AopSdk.F2FPay.AspnetCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Vapps.Alipay.Infrastructure
{

    public static class AlipayRegistrar
    {
        public static void Register(IServiceCollection services, IConfiguration configuration)
        {
            services.AddAlipay(options =>
            {
                options.AlipayPublicKey = configuration[ConfigKeys.PublicKey];
                options.PrivateKey = configuration[ConfigKeys.PrivateKey];
                options.AppId = configuration[ConfigKeys.AppId];
                options.CharSet = configuration[ConfigKeys.CharSet];
                options.Gatewayurl = configuration[ConfigKeys.Gatewayurl];
                options.SignType = configuration[ConfigKeys.SignType];
                options.Uid = configuration[ConfigKeys.Pid];
            }).AddAlipayF2F();
        }
    }
}
