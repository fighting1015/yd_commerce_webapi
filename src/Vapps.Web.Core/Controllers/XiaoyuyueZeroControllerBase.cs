using Abp.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Identity;
using Vapps.Identity;

namespace Vapps.Web.Controllers
{
    public abstract class VappsControllerBase : AbpController
    {
        protected VappsControllerBase()
        {
            LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }

        protected void CheckErrors(IdentityResult identityResult)
        {
            identityResult.CheckErrors(LocalizationManager);
        }
    }
}