using Abp.BackgroundJobs;
using Abp.ObjectMapping;
using Abp.Web.Models;
using Alipay.AopSdk.AspnetCore;
using Alipay.AopSdk.Core.Response;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Alipay.Infrastructure;
using Vapps.Alipay.Infrastructure.Payments.Jobs;

namespace Vapps.Alipay.Application.Payments
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class AlipayPaymentAppService : VappsAppServiceBase, IAlipayPaymentAppService
    {
        private readonly ILogger _logger;
        private readonly IObjectMapper _objectMapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly IAlipayService _alipayService;
        private readonly AlipayConfiguration _configuration;

        public AlipayPaymentAppService(ILogger logger,
            IObjectMapper objectMapper,
            IHttpContextAccessor httpContext,
            IBackgroundJobManager backgroundJobManager,
            IAlipayService alipayService,
            AlipayConfiguration configuration)
        {
            this._logger = logger;
            this._objectMapper = objectMapper;
            this._httpContext = httpContext;
            this._configuration = configuration;
            this._backgroundJobManager = backgroundJobManager;
            this._alipayService = alipayService;
        }

        /// <summary>
        /// 支付宝支付异步通知
        /// </summary>
        /// <returns></returns>
        [DontWrapResult]
        [HttpPost]
        public async Task<string> Notify()
        {
            /* 实际验证过程建议商户添加以下校验。
           1、商户需要验证该通知数据中的out_trade_no是否为商户系统中创建的订单号，
           2、判断total_amount是否确实为该订单的实际金额（即商户订单创建时的金额），
           3、校验通知中的seller_id（或者seller_email) 是否为out_trade_no这笔单据的对应的操作方（有的时候，一个商户可能有多个seller_id/seller_email）
           4、验证app_id是否为该商户本身。
           */
            Dictionary<string, string> sArray = GetRequestPost();
            string result = GetJsonString(sArray);

            AlipayTradePayResponse response = JsonConvert.DeserializeObject<AlipayTradePayResponse>(result);
            if (sArray.Count != 0)
            {
                //验证请求是否从支付宝发过来（安全）
                bool flag = _alipayService.RSACheckV1(sArray);
                if (flag)
                {
                    //交易状态
                    //判断该笔订单是否在商户网站中已经做过处理
                    //如果没有做过处理，根据订单号（out_trade_no）在商户网站的订单系统中查到该笔订单的详细，并执行商户的业务程序
                    //请务必判断请求时的total_amount与通知时获取的total_fee为一致的
                    //如果有做过处理，不执行商户的业务程序

                    await _backgroundJobManager.EnqueueAsync<ProcessAlipayPaymentJob, AlipayTradePayResponse>(response);

                    //注意：
                    //退款日期超过可退款期限后（如三个月可退款），支付宝系统发送该交易状态通知
                    //Console.WriteLine(Request.Form["trade_status"]);

                    return await Task.FromResult("success");
                }
                else
                {
                    return await Task.FromResult("fail");
                }
            }
            return await Task.FromResult("fail");
        }

        private static string GetJsonString(Dictionary<string, string> sArray)
        {
            return JsonConvert.SerializeObject(sArray).Replace(@"\", string.Empty).Replace("\"[", "[").Replace("]\"", "]");
        }

        private Dictionary<string, string> GetRequestPost()
        {
            Dictionary<string, string> sArray = new Dictionary<string, string>();

            ICollection<string> requestItem = _httpContext.HttpContext.Request.Form.Keys;
            foreach (var item in requestItem)
            {
                sArray.Add(item, _httpContext.HttpContext.Request.Form[item]);

            }
            return sArray;

        }
    }
}
