using System.Threading.Tasks;
using Vapps.ExternalAuthentications;
using Vapps.Web.Models.TokenAuth;
using Vapps.WeChat.Authentication;

namespace Vapps.Web.Authentication.External.Wechat
{
    public class WeChatAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string Name = "WeChat";

        public readonly WeChatExternalAuthManager _weChatExternalAuthManager;

        public WeChatAuthProviderApi(WeChatExternalAuthManager weChatExternalAuthManager)
        {
            this._weChatExternalAuthManager = weChatExternalAuthManager;

        }

        public override async Task<ExternalLoginUserInfo> GetUserInfo(string accessCode)
        {
            var userInfo = await _weChatExternalAuthManager.GetUserInfo(accessCode, Name);

            return userInfo;
        }
    }

    public class WeChatMPAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string Name = "WeChatMP";

        public readonly WeChatExternalAuthManager _weChatExternalAuthManager;

        public WeChatMPAuthProviderApi(WeChatExternalAuthManager weChatExternalAuthManager)
        {
            this._weChatExternalAuthManager = weChatExternalAuthManager;
        }

        public override async Task<ExternalLoginUserInfo> GetUserInfo(string accessCode)
        {
            var userInfo = await _weChatExternalAuthManager.GetUserInfo(accessCode, Name);
            return userInfo;
        }
    }
}
