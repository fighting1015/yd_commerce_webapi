using Abp.Dependency;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.ExternalAuthentications;
using Vapps.WeChat.Authentication.LoginProvider;

namespace Vapps.WeChat.Authentication
{
    public partial class WeChatExternalAuthManager : ITransientDependency
    {
        private readonly WebLoginProvider _webLoginProvider;
        private readonly WebMPLoginProvider _webMPLoginProvider;

        public WeChatExternalAuthManager(
            WebLoginProvider webLoginProvider,
            WebMPLoginProvider webMPLoginProvider)
        {
            this._webLoginProvider = webLoginProvider;
            this._webMPLoginProvider = webMPLoginProvider;
        }

        public async Task<ExternalLoginUserInfo> GetUserInfo(string code, string providerName)
        {
            /* 根据登陆入口类型，获取微信用户资料
             * TODO:保存AccessToken到数据库中
             */
            if (providerName == WeChatConsts.MPNAME)
            {
                //公众号内授权
                return await _webMPLoginProvider.GetUserInfo(code);
            }
            else
            {
                return await _webLoginProvider.GetUserInfo(code);
            }
        }
    }
}
