using Abp.Dependency;
using Senparc.Weixin;
using Senparc.Weixin.Exceptions;
using Senparc.Weixin.MP.Containers;
using System.Threading.Tasks;

namespace Vapps.WeChat.Operation
{
    public class BaseOperation
    {
        public string AppId { get; set; }
        public string AppSecret { get; set; }

        public void Init(string clientKeyIdentifier, string clientSecret)
        {
            this.AppId = clientKeyIdentifier;
            this.AppSecret = clientSecret;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        public async Task CheckAccessTokenExpiresAsync(ErrorJsonResultException ex)
        {
            if (ex.JsonResult.errcode == ReturnCode.access_token超时)
                await GetAccessTokenAsync(true);
        }

        /// <summary>
        /// 获取AccessToken
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetAccessTokenAsync(bool getNewToken = false)
        {
            return await AccessTokenContainer.TryGetAccessTokenAsync(AppId, AppSecret, getNewToken);
        }

        /// <summary>
        /// 获取CardApiTicket
        /// </summary>
        /// <returns></returns>
        public async Task<string> GetCardApiTicketAsync(bool getNewToken = false)
        {
            return await JsApiTicketContainer.TryGetJsApiTicketAsync(AppId, AppSecret, getNewToken);
        }
    }
}
