using Abp.Dependency;
using Abp.UI;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.Editions;
using Vapps.Helpers;
using Vapps.Payments;
using Vapps.WeChat.Core.Users;
using Senparc.Weixin.TenPay.V3;
using Senparc.Weixin.TenPay;

namespace Vapps.WeChat.Payments
{
    public class WeChatPaymentGatewayProvider : VappsServiceBase, IPaymentGatewayProvider
    {
        public string Name => "WeChat";

        public bool IsEnable => _configuration.IsEnable;

        private readonly IWebHelper _webHelper;
        private readonly WeChatUserManager _weChatUserManager;
        private readonly WeChatConfiguration _configuration;
        private readonly WeChatCommonHepler _weChatCommonHepler;

        public WeChatPaymentGatewayProvider(IWebHelper webHelper,
            WeChatUserManager weChatUserManager,
            WeChatConfiguration configuration,
            WeChatCommonHepler weChatCommonHepler)
        {
            _webHelper = webHelper;
            _weChatUserManager = weChatUserManager;
            _configuration = configuration;
            _weChatCommonHepler = weChatCommonHepler;
        }

        public async Task<CreatePaymentResponse> CreatePaymentAsync(CreatePaymentRequest request)
        {
            string tenPayNotify = string.Concat(_configuration.ServerAddress, _configuration.NotifyUrl);
            var nonceStr = TenPayV3Util.GetNoncestr();

            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(_configuration.AppId,
                _configuration.MerchantId,
                request.Description,
                request.PaymentId,
                Convert.ToInt32(request.Amount * 100),
                _webHelper.GetCurrentIpAddress(),
                tenPayNotify,
                TenPayV3Type.NATIVE,
                null,
                _configuration.TenPayKey,
                nonceStr);

            string data = packageReqHandler.ParseXML();

            //调用统一订单接口
            var tenPayresult = TenPayV3.Unifiedorder(xmlDataInfo, int.MaxValue);

            var result = new CreatePaymentResponse();
            result.AdditionalData.Add("CodeUrl", tenPayresult.code_url);

            return await Task.FromResult(result);
        }

        public async Task<CreatePaymentResponse> CreateJsPaymentAsync(CreatePaymentRequest request)
        {
            var result = new CreatePaymentResponse();

            string timeStamp = TenPayV3Util.GetTimestamp();
            string nonceStr = TenPayV3Util.GetNoncestr();
            string tenPayNotify = string.Concat(_configuration.ServerAddress, _configuration.NotifyUrl);

            var weChatUser = await GetWeChatUser(request.UserId.Value);
            if (weChatUser == null)
                throw new UserFriendlyException(L("Payments.WeChat.CanNotFindUser"));

            //创建支付应答对象
            RequestHandler packageReqHandler = new RequestHandler(null);
            var xmlDataInfo = new TenPayV3UnifiedorderRequestData(_configuration.AppId,
                _configuration.MerchantId,
                request.Description,
                request.PaymentId,
                Convert.ToInt32(request.Amount * 100),
                _webHelper.GetCurrentIpAddress(),
                tenPayNotify,
                TenPayV3Type.JSAPI,
                weChatUser.OpenId,
                _configuration.TenPayKey,
                nonceStr);

            var tenPayresult = TenPayV3.Unifiedorder(xmlDataInfo);
            var package = string.Format("prepay_id={0}", tenPayresult.prepay_id);

            var appInfo = await _weChatCommonHepler.GetLoginProviderSettingAsync(WeChatConsts.MPNAME);
            var paySign = TenPayV3.GetJsPaySign(_configuration.AppId, timeStamp, nonceStr, package, _configuration.TenPayKey);

            result.AdditionalData.Add("appId", _configuration.AppId);
            result.AdditionalData.Add("timeStamp", timeStamp);
            result.AdditionalData.Add("nonceStr", nonceStr);
            result.AdditionalData.Add("package", package);
            result.AdditionalData.Add("paySign", paySign);

            return result;
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

        private async Task<WeChatUser> GetWeChatUser(long userId)
        {
            var wechatUser = await _weChatUserManager.FindByUserIdAndMpIdAsync(userId, 0);
            return wechatUser;
        }
    }
}
