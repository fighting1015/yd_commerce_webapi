using Abp.Configuration;
using Abp.Dependency;
using Abp.Localization;
using Abp.Timing;
using Abp.UI;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.Enums;
using Vapps.ExternalAuthentications;
using Vapps.WeChat.Operation;

namespace Vapps.WeChat.Authentication.LoginProvider
{
    /// <summary>
    /// 微信开放平台登陆
    /// </summary>
    public partial class WebMPLoginProvider : ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly ILocalizationManager _localizationManager;
        private readonly WeChatCommonHepler _wechatCommonHepler;

        public WebMPLoginProvider(IHostingEnvironment env,
            ISettingManager settingManager,
            ILocalizationManager localizationManager,
            WeChatCommonHepler wechatCommonHepler,
            UserAccountOperation userAccountOperation)
        {
            this._logger = NullLogger.Instance;
            this._wechatCommonHepler = wechatCommonHepler;
            this._localizationManager = localizationManager;
        }

        /// <summary>
        /// 获取微信用户信息
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ExternalLoginUserInfo> GetUserInfo(string code)
        {
            var loginProviderSetting = _wechatCommonHepler.GetLoginProviderSettingAsync(WeChatConsts.MPNAME).Result;

            var loginUserInfo = new ExternalLoginUserInfo();
            try
            {
                //获取AccessToken
                var accessTokenResult = await OAuthApi.GetAccessTokenAsync(loginProviderSetting.AppId, loginProviderSetting.AppSecret, code);
                if (accessTokenResult.errcode == ReturnCode.请求成功)
                {
                    //获取用户资料
                    var userInfo = await OAuthApi.GetUserInfoAsync(accessTokenResult.access_token, accessTokenResult.openid);
                    loginUserInfo.EmailAddress = string.Empty;
                    loginUserInfo.Name = userInfo.nickname;
                    loginUserInfo.Surname = string.Empty;
                    loginUserInfo.AccessToken = accessTokenResult.access_token;
                    loginUserInfo.AccessTokenOutDataTime = Clock.Now.AddSeconds(accessTokenResult.expires_in);
                    loginUserInfo.RefreshToken = accessTokenResult.refresh_token;
                    loginUserInfo.Provider = WeChatConsts.WEBPAGENAME;
                    loginUserInfo.ProviderKey = userInfo.openid;
                    loginUserInfo.UnionProviderKey = userInfo.unionid;
                    loginUserInfo.Gender = (GenderType)userInfo.sex;
                    loginUserInfo.Country = userInfo.country;
                    loginUserInfo.Province = userInfo.province;
                    loginUserInfo.City = userInfo.city;
                    loginUserInfo.ProfilePictureUrl = userInfo.headimgurl;
                }

                return loginUserInfo;
            }
            catch (ErrorJsonResultException ex)
            {
                _logger.Error(ex.Message, ex);
                throw new UserFriendlyException(L("Wechat.Authentication.Error"));
            }
        }

        private string L(string name)
        {
            return _localizationManager.GetString(VappsConsts.ServerSideLocalizationSourceName, name);
        }
    }
}
