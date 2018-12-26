using Alipay.AopSdk.F2FPay.AspnetCore;
using Alipay.AopSdk.F2FPay.Business;
using Alipay.AopSdk.F2FPay.Domain;
using Alipay.AopSdk.F2FPay.Model;
using Castle.Core.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Editions;
using Vapps.Helpers;
using Vapps.Payments;

namespace Vapps.Alipay.Infrastructure.Payments
{
    public class AlipayPaymentGatewayProvider : VappsServiceBase, IPaymentGatewayProvider
    {
        public string Name => "Alipay";

        public bool IsEnable => _configuration.IsEnable;

        private readonly IAlipayF2FService _alipayF2FService;
        private readonly IWebHelper _webHelper;
        private readonly ILogger _logger;
        private readonly AlipayConfiguration _configuration;

        public AlipayPaymentGatewayProvider(IAlipayF2FService alipayF2FService,
            IWebHelper webHelper,
            ILogger logger,
            AlipayConfiguration configuration)
        {
            _alipayF2FService = alipayF2FService;
            _webHelper = webHelper;
            _logger = logger;
            _configuration = configuration;
            LocalizationSourceName = VappsConsts.ServerSideLocalizationSourceName;
        }

        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            var result = new CreatePaymentResponse();
            AlipayTradePrecreateContentBuilder builder = BuildPrecreateContent(request.PaymentId, request.Description, request.Amount);

            //支付回调
            string tenPayNotify = string.Concat(_configuration.ServerAddress, _configuration.NotifyUrl);
            AlipayF2FPrecreateResult precreateResult = _alipayF2FService.TradePrecreate(builder, tenPayNotify);
            //AlipayF2FPrecreateResult precreateResult = _alipayF2FService.TradePrecreate(builder);
            switch (precreateResult.Status)
            {
                case ResultEnum.SUCCESS:
                    result.AdditionalData.Add("CodeUrl", precreateResult.response.QrCode);
                    break;
                default:
                    _logger.Error(L("Payments.Alipay.PayFail", precreateResult.response.SubCode, precreateResult.response.SubMsg));
                    break;
            }

            return await Task.FromResult(result);
        }

        public async Task<CreatePaymentResponse> CreateJsPaymentAsync(CreatePaymentRequest request)
        {
            var result = new CreatePaymentResponse();

            return await Task.FromResult(result);
        }

        public async Task<QueryPaymentResponse> QueryPaymentAsync(string paymentId)
        {
            return await Task.FromResult(new QueryPaymentResponse());
        }

        public Task<Dictionary<string, string>> GetAdditionalPaymentData(SubscribableEdition edition)
        {
            Dictionary<string, string> additionalData = new Dictionary<string, string>();

            return Task.FromResult(additionalData);
        }

        /// <summary>
	    /// 构造支付请求数据
	    /// </summary>
	    /// <param name="orderName">订单名称</param>
	    /// <param name="orderAmount">订单金额</param>
	    /// <param name="outTradeNo">订单编号</param>
	    /// <returns>请求结果集</returns>
	    private AlipayTradePrecreateContentBuilder BuildPrecreateContent(string paymentId, string description, decimal amount)
        {
            AlipayTradePrecreateContentBuilder builder = new AlipayTradePrecreateContentBuilder();
            //收款账号
            //builder.seller_id = _configuration.Uid;
            //订单编号
            builder.out_trade_no = paymentId;
            //订单总金额
            builder.total_amount = amount.ToString();
            //参与优惠计算的金额
            //builder.discountable_amount = "";
            //不参与优惠计算的金额
            //builder.undiscountable_amount = "";
            //订单名称
            builder.subject = description;
            //自定义超时时间
            builder.timeout_express = "2h";
            //订单描述
            builder.body = "";
            //门店编号，很重要的参数，可以用作之后的营销
            //builder.store_id = "test store id";
            //操作员编号，很重要的参数，可以用作之后的营销
            //builder.operator_id = "test";

            //传入商品信息详情
            List<GoodsInfo> gList = new List<GoodsInfo>();
            GoodsInfo goods = new GoodsInfo();
            goods.goods_id = description;
            goods.goods_name = description;
            goods.price = amount.ToString();
            goods.quantity = "1";
            gList.Add(goods);
            builder.goods_detail = gList;

            //系统商接入可以填此参数用作返佣
            //ExtendParams exParam = new ExtendParams();
            //exParam.sysServiceProviderId = "20880000000000";
            //builder.extendParams = exParam;

            return builder;

        }
    }
}
