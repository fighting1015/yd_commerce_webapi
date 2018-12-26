using System.Threading.Tasks;
using Vapps.WeChat.Application.JSSDK.Dto;

namespace Vapps.WeChat.Application.JSSDK
{
    public interface IWeChatJSAppService
    {
        /// <summary>
        /// 获取Js-sdk签名
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetJsApiSignatureOutput> GetJsApiSignature(GetJsApiSignatureInput input);
    }
}
