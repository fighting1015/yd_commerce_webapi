using Abp.Dependency;
using Abp.Timing;
using Castle.Core.Logging;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.AdvancedAPIs;
using Senparc.Weixin.MP.AdvancedAPIs.OAuth;
using System.Threading.Tasks;
using Vapps.ExternalAuthentications;

namespace Vapps.WeChat.Operation
{
    /// <summary>
    /// 微信用户操作类
    /// </summary>
    public class UserAccountOperation : ITransientDependency
    {
        private readonly ILogger _logger;
        private readonly CommonOperation _commonOperation;

        public UserAccountOperation(ILogger logger,
            CommonOperation commonOperation)
        {
            this._logger = logger;
            this._commonOperation = commonOperation;
        }

        /// <summary>
        /// 获取微信用户信息
        /// TODO:保存微信用户资料到数据库中
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async Task<OAuthUserInfo> GetUserInfoAsync(string code)
        {
            OAuthUserInfo userInfo = null;
            try
            {
                //获取AccessToken
                var accessTokenResult = await OAuthApi.GetAccessTokenAsync(_commonOperation.AppId, _commonOperation.AppSecret, code);
                if (accessTokenResult.errcode == ReturnCode.请求成功)
                {
                    //获取用户资料
                    userInfo = await OAuthApi.GetUserInfoAsync(accessTokenResult.access_token, accessTokenResult.openid);
                    //loginUserInfo.EmailAddress = string.Empty;
                    //loginUserInfo.Name = userInfo.nickname;
                    //loginUserInfo.Surname = string.Empty;
                    //loginUserInfo.AccessToken = accessTokenResult.access_token;
                    //loginUserInfo.AccessTokenOutDataTime = Clock.Now.AddSeconds(accessTokenResult.expires_in);
                    //loginUserInfo.RefreshToken = accessTokenResult.refresh_token;
                    //loginUserInfo.Provider = WeChatConsts.WEBPAGENAME;
                    //loginUserInfo.ProviderKey = userInfo.openid;
                }

                return userInfo;

            }
            catch (ErrorJsonResultException ex)
            {
                _logger.Error(ex.Message, ex);
                return userInfo;
            }
        }
    }
}
