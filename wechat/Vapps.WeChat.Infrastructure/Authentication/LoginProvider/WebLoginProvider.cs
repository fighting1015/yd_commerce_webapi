using Abp.Dependency;
using Abp.Localization;
using Abp.Timing;
using Abp.UI;
using Castle.Core.Logging;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.Open.QRConnect;
using System.Threading.Tasks;
using Vapps.Enums;
using Vapps.ExternalAuthentications;

namespace Vapps.WeChat.Authentication.LoginProvider
{
    /// <summary>
    /// 微信开放平台登陆
    /// </summary>
    public partial class WebLoginProvider : ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly ILocalizationManager _localizationManager;
        private readonly WeChatCommonHepler _wechatCommonHepler;

        public WebLoginProvider(ILocalizationManager localizationManager,
            WeChatCommonHepler wechatCommonHepler)
        {
            this._localizationManager = localizationManager;
            this._wechatCommonHepler = wechatCommonHepler;
            this._logger = NullLogger.Instance;
        }

        /// <summary>
        /// 获取微信用户信息
        /// TODO:保存微信用户资料到数据库中
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<ExternalLoginUserInfo> GetUserInfo(string code)
        {
            var loginUserInfo = new ExternalLoginUserInfo();
            var loginProviderSetting = await _wechatCommonHepler.GetLoginProviderSettingAsync(WeChatConsts.WEBPAGENAME);

            try
            {
                //获取AccessToken
                var accessTokenResult = await QRConnectAPI.GetAccessTokenAsync(loginProviderSetting.AppId,
                   loginProviderSetting.AppSecret, code);

                if (accessTokenResult.errcode == ReturnCode.请求成功)
                {
                    //获取用户资料
                    var userInfo = await QRConnectAPI.GetUserInfoAsync(accessTokenResult.access_token, accessTokenResult.openid);
                    loginUserInfo.UserName = userInfo.openid;
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
