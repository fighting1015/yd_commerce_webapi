using Abp.ObjectMapping;
using Abp.Web.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.MP.TenPayLibV3;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xiaoyuyue.Editions;
using Xiaoyuyue.MultiTenancy;
using Xiaoyuyue.Payments;
using Xiaoyuyue.Payments.Cache;
using Xiaoyuyue.WeChat.Payment;

namespace Xiaoyuyue.Web.Controllers
{
    [Route("api/[controller]/[action]")]
    public class PaymentController : XiaoyuyueControllerBase

    {
        private readonly ILogger _logger;
        private readonly IPaymentIdCache _paymentIdCache;
        private readonly ISubscriptionPaymentCache _subscriptionPaymentCache;
        private readonly ISubscriptionPaymentManager _subscriptionPaymentManager;
        private readonly IObjectMapper _objectMapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly TenantManager _tenantManager;
        private readonly WeChatConfiguration _configuration;

        public PaymentController(ILogger logger,
            IPaymentIdCache paymentIdCache,
            ISubscriptionPaymentCache subscriptionPaymentCache,
            ISubscriptionPaymentManager subscriptionPaymentManager,
            IObjectMapper objectMapper,
            IHttpContextAccessor httpContext,
            TenantManager tenantManager,
            WeChatConfiguration configuration)
        {
            this._logger = logger;
            this._paymentIdCache = paymentIdCache;
            this._subscriptionPaymentCache = subscriptionPaymentCache;
            this._subscriptionPaymentManager = subscriptionPaymentManager;
            this._objectMapper = objectMapper;
            this._httpContext = httpContext;
            this._configuration = configuration;
            this._tenantManager = tenantManager;
        }

        /// <summary>
        /// 支付支付异步通知
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public async Task<string> Notify()
        {
            ResponseHandler resHandler = new ResponseHandler(null);

            string openId = resHandler.GetParameter("openid");

            resHandler.SetKey(_configuration.TenPayKey);
            OrderQueryResult result = new OrderQueryResult(resHandler.ParseXML());

            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign())
            {
                if (result.IsReturnCodeSuccess())
                {
                    //------------------------------
                    //处理业务开始
                    //------------------------------
                    //处理数据库逻辑
                    //注意交易单不要重复处理
                    //注意判断返回金额
                    var paymentId = result.out_trade_no;

                    var subscriptionPaymentCache = _subscriptionPaymentCache.GetCacheItemOrNull(paymentId);
                    if (subscriptionPaymentCache == null)
                    {
                        _logger.Error(L("Payments.WeChat.PayFail.PaymentIdNotFound", paymentId));
                    }
                    else if ((subscriptionPaymentCache.Amount * 100).ToString() != result.total_fee)
                    {
                        _logger.Error(L("Payments.WeChat.PayFail.PaymentAmountNotMatch", paymentId));
                    }
                    else
                    {
                        var subscriptionPayment = _objectMapper.Map<SubscriptionPayment>(subscriptionPaymentCache);
                        subscriptionPayment.Gateway = SubscriptionPaymentGatewayType.WeChat;
                        subscriptionPayment.Status = SubscriptionPaymentStatus.Completed;
                        await _subscriptionPaymentManager.CreateAsync(subscriptionPayment);

                        using (CurrentUnitOfWork.SetTenantId(subscriptionPayment.TenantId))
                        {
                            await _tenantManager.UpdateTenantAsync(
                                subscriptionPayment.TenantId,
                                true,
                                false,
                                subscriptionPayment.PaymentPeriodType,
                                subscriptionPayment.EditionId,
                                EditionPaymentType.BuyNow
                            );

                            _paymentIdCache.RemoveCacheItem(subscriptionPayment.TenantId,
                                subscriptionPayment.EditionId,
                                subscriptionPayment.EditionPaymentType,
                                subscriptionPayment.PaymentPeriodType,
                                subscriptionPayment.Gateway);

                            _subscriptionPaymentCache.RemoveCacheItem(subscriptionPaymentCache.PaymentId);
                        }
                    }
                }
                else
                {
                    _logger.Error(L("Payments.WeChat.PayFail", result.err_code, result.err_code_des));
                }
            }

            string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[{1}]]></return_msg></xml>", result.err_code, result.err_code_des);
            return await Task.FromResult(xml);
        }
    }

}
