using Castle.Core.Logging;
using Senparc.Weixin.MP.Helpers;
using System;
using System.Threading.Tasks;
using Vapps.WeChat.Application.JSSDK;
using Vapps.WeChat.Application.JSSDK.Dto;
using Vapps.WeChat.Operation;

namespace Vapps.WeChat.Application
{
    public class WeChatJSAppService : VappsAppServiceBase, IWeChatJSAppService
    {
        private readonly ILogger _logger;
        private readonly CommonOperation _commonOperation;

        public WeChatJSAppService(ILogger logger,
            CommonOperation commonOperation)
        {
            this._logger = logger;
            this._commonOperation = commonOperation;
        }

        /// <summary>
        /// 获取Js-sdk签名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetJsApiSignatureOutput> GetJsApiSignature(GetJsApiSignatureInput input)
        {
            var output = new GetJsApiSignatureOutput();
            try
            {
                output.Signature = JSSDKHelper.GetSignature(await _commonOperation.GetCardApiTicketAsync(),
                    input.NonceStr, input.Timestamp, input.SourceUrl.Utf8Encode());
                output.AppId = _commonOperation.AppId;
            }
            catch (Exception ex)
            {
                _logger.Error(ex.Message);
            }
            return output;
        }
    }
}
