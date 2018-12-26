using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Threading;
using System;
using System.Net.Http;
using Vapps.Url;

namespace Vapps.Common
{
    public class KeepAliveJob : BackgroundJob<bool>, ITransientDependency
    {
        private readonly IWebUrlService _webUrlService;

        public KeepAliveJob(IWebUrlService webUrlService)
        {
            this._webUrlService = webUrlService;
        }

        public override void Execute(bool status)
        {
            AsyncHelper.RunSync(async () =>
            {
                try
                {
                    var getIpInfoUrl = $"{_webUrlService.GetServerRootAddress()}/AbpUserConfiguration/GetAll";
                    using (var client = new HttpClient())
                    {
                        client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                        client.Timeout = TimeSpan.FromSeconds(120);
                        client.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

                        var response = await client.GetAsync(getIpInfoUrl);
                    }
                }
                catch (Exception)
                {
                }
            });
        }
    }
}
