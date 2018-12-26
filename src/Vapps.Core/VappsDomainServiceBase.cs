using Abp.Domain.Services;

namespace Vapps
{
    public abstract class VappsDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */

        protected VappsDomainServiceBase()
        {
            LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }
    }
}
