using Abp.Configuration;
using Abp.Timing;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Vapps.Configuration;
using Vapps.ExternalAuthentications;
using Vapps.Web.Models.TokenAuth;

namespace Vapps.Web.Authentication.External.QQ
{
    public class QQAuthProviderApi : ExternalAuthProviderApiBase
    {
        public const string Name = "QQ";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string _tokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string _userIdEndpoint = "https://graph.qq.com/oauth2.0/me";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string _userInfoEndpoint = "https://graph.qq.com/user/get_user_info";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string _refresh_TokenEndpoint = "https://graph.qq.com/oauth2.0/token";

        /// <summary>
        /// The token endpoint.
        /// </summary>
        private const string _returnUrl = "https://business.vapps.com.cn/auth/external";

        private readonly ISettingManager _settingManager;
        private readonly ExternalAuthenticationProvider _provider;

        public QQAuthProviderApi(ISettingManager settingManager)
        {
            this._settingManager = settingManager;
            this._provider = GetLoginProviderSettingAsync();
        }

        public override async Task<ExternalLoginUserInfo> GetUserInfo(string accessCode)
        {
            var accessTokenDto = await QueryAccessToken(accessCode);
            var openId = await QueryOpenId(accessTokenDto.AccessToken);
            var userInfo = await QueryUserInfo(accessTokenDto.AccessToken, openId);
            userInfo.AccessToken = accessTokenDto.AccessToken;
            userInfo.ProviderKey = openId;
            userInfo.UnionProviderKey = openId;
            userInfo.RefreshToken = accessTokenDto.RefreshToken;
            userInfo.AccessTokenOutDataTime = Clock.Now.AddSeconds(accessTokenDto.ExpiresIn);
            userInfo.UserName = openId;
            return userInfo;
        }

        public async Task<QQAccessTokenDto> QueryAccessToken(string authorizationCode)
        {
            var endpoint = QueryHelpers.AddQueryString(_tokenEndpoint, "grant_type", "authorization_code");
            endpoint = QueryHelpers.AddQueryString(endpoint, "client_id", _provider.AppId);
            endpoint = QueryHelpers.AddQueryString(endpoint, "client_secret", _provider.AppSecret);
            endpoint = QueryHelpers.AddQueryString(endpoint, "code", authorizationCode);
            endpoint = QueryHelpers.AddQueryString(endpoint, "redirect_uri", _returnUrl);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth middleware");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                client.DefaultRequestHeaders.Host = "graph.qq.com";
                client.Timeout = TimeSpan.FromSeconds(30);
                client.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                string result = await response.Content.ReadAsStringAsync();
                NameValueCollection nameValueCollection = HttpUtility.ParseQueryString(result);
                var payload = new QQAccessTokenDto()
                {
                    AccessToken = nameValueCollection["access_token"],
                    RefreshToken = nameValueCollection["refresh_token"],
                    ExpiresIn = Convert.ToInt32(nameValueCollection["expires_in"])
                };

                return payload;
            }
        }

        public async Task<string> QueryOpenId(string accessToken)
        {
            var endpoint = QueryHelpers.AddQueryString(_userIdEndpoint, "access_token", accessToken);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth middleware");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                client.DefaultRequestHeaders.Host = "graph.qq.com";
                client.Timeout = TimeSpan.FromSeconds(30);
                client.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                result = result.Replace("callback(", "");
                result = result.Replace(");\n", "");
                var payload = JObject.Parse(result);

                return payload.Value<string>("openid");
            }
        }

        public async Task<ExternalLoginUserInfo> QueryUserInfo(string accessToken, string openid)
        {
            var endpoint = QueryHelpers.AddQueryString(_userInfoEndpoint, "access_token", accessToken);
            endpoint = QueryHelpers.AddQueryString(endpoint, "oauth_consumer_key", _provider.AppId);
            endpoint = QueryHelpers.AddQueryString(endpoint, "openid", openid);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.UserAgent.ParseAdd("Microsoft ASP.NET Core OAuth middleware");
                client.DefaultRequestHeaders.Accept.ParseAdd("application/json");
                client.DefaultRequestHeaders.Host = "graph.qq.com";
                client.Timeout = TimeSpan.FromSeconds(30);
                client.MaxResponseContentBufferSize = 1024 * 1024 * 10; // 10 MB

                var response = await client.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                var payload = JObject.Parse(result);

                return new ExternalLoginUserInfo
                {
                    Provider = Name,
                    Name = QQHelper.GetNickname(payload),
                    Surname = QQHelper.GetNickname(payload),
                    ProfilePictureUrl = QQHelper.GetProfilePicture(payload),
                    Gender = QQHelper.GetGender(payload),
                    City = QQHelper.GetCity(payload),
                    Province = QQHelper.GetProvince(payload),
                };
            }
        }

        /// <summary>
        /// 获取微信登陆设置
        /// </summary>
        /// <returns></returns>
        public virtual ExternalAuthenticationProvider GetLoginProviderSettingAsync()
        {
            var loginProviderInfo = _settingManager
                .GetSettingValue(string.Format(AppSettings.ExternalAuthentication.ProviderName, Name));

            ExternalAuthenticationProvider result = new ExternalAuthenticationProvider();
            if (!loginProviderInfo.IsNullOrEmpty())
            {
                result = JsonConvert.DeserializeObject<ExternalAuthenticationProvider>(loginProviderInfo);
            }

            return result;
        }
    }
}
