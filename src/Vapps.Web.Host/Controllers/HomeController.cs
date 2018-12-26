using Abp.Auditing;
using Microsoft.AspNetCore.Mvc;

namespace Vapps.Web.Controllers
{
    public class HomeController : VappsControllerBase
    {
        [DisableAuditing]
        public IActionResult Index()
        {
            return Redirect("/swagger");
        }
    }
}
