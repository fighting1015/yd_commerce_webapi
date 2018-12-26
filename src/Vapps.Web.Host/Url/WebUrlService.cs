using Abp.Dependency;
using Vapps.Configuration;
using Vapps.Url;

namespace Vapps.Web.Url
{
    public class WebUrlService : WebUrlServiceBase, IWebUrlService, ITransientDependency
    {
        public WebUrlService(
           IAppConfigurationAccessor configurationAccessor) :
           base(configurationAccessor)
        {
        }

        public override string WebSiteRootAddressFormatKey => "App:ClientRootAddress";

        public override string UserCenterAddressFormatKey => "App:UserCenterAddress";

        public override string BusinessCenterAddressFormatKey => "App:BusinessCenterAddress";

        public override string ServerRootAddressFormatKey => "App:ServerRootAddress";
    }
}
