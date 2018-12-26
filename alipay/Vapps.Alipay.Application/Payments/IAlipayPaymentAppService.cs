using Abp.Web.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Alipay.Application.Payments
{
    /// <summary>
    /// 支付宝支付
    /// </summary>
    public interface IAlipayPaymentAppService
    {
        /// <summary>
        /// 支付宝支付异步通知
        /// </summary>
        /// <returns></returns>
        [DontWrapResult]
        Task<string> Notify();
    }
}
