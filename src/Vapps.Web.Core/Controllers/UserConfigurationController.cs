using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Vapps.Configuration;

namespace Vapps.Web.Controllers
{
    /// <summary>
    /// 用户设置
    /// </summary>
    [Route("api/[controller]/[action]")]
    public class UserConfigurationController : VappsControllerBase
    {
        private readonly UserConfigurationBuilder _userConfigurationBuilder;

        public UserConfigurationController(UserConfigurationBuilder userConfigurationBuilder)
        {
            _userConfigurationBuilder = userConfigurationBuilder;
        }

        /// <summary>
        /// 获取所有设置
        /// </summary>
        /// <param name="sourceName"></param>
        /// <returns></returns>
        [HttpGet]
        public async Task<JsonResult> GetAll(string sourceName)
        {
            var userConfig = await _userConfigurationBuilder.GetAll(sourceName);
            return Json(userConfig);
        }
    }
}
