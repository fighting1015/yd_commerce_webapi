using Abp.AutoMapper;
using Vapps.Web.Authentication.External;

namespace Vapps.Web.Models.TokenAuth
{
    [AutoMapFrom(typeof(ExternalLoginProviderInfo))]
    public class ExternalLoginProviderInfoModel
    {
        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 客户端Id
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// 是否在登录界面显示
        /// </summary>
        public bool ShowOnLoginPage { get; set; }
    }
}
