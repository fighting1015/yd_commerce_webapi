using System.Threading.Tasks;
using Abp.Web.Models;

namespace Vapps.WeChat.Application.Payments
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public interface IWeChatPaymentAppService
    {
        /// <summary>
        /// 微信支付异步通知
        /// </summary>
        /// <returns></returns>
        [DontWrapResult]
        Task<string> Notify();
    }
}
