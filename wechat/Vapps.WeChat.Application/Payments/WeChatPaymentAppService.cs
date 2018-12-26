using Abp.BackgroundJobs;
using Abp.ObjectMapping;
using Abp.Web.Models;
using Castle.Core.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Senparc.Weixin.TenPay.V3;
using System.Threading.Tasks;
using Vapps.WeChat.Payments.Jobs;

namespace Vapps.WeChat.Application.Payments
{
    /// <summary>
    /// 微信支付
    /// </summary>
    public class WeChatPaymentAppService : VappsAppServiceBase, IWeChatPaymentAppService
    {
        private readonly ILogger _logger;
        private readonly IObjectMapper _objectMapper;
        private readonly IHttpContextAccessor _httpContext;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly WeChatConfiguration _configuration;

        public WeChatPaymentAppService(ILogger logger,
            IObjectMapper objectMapper,
            IHttpContextAccessor httpContext,
            IBackgroundJobManager backgroundJobManager,
            WeChatConfiguration configuration)
        {
            this._logger = logger;
            this._objectMapper = objectMapper;
            this._httpContext = httpContext;
            this._configuration = configuration;
            this._backgroundJobManager = backgroundJobManager;
        }

        /// <summary>
        /// 微信支付异步通知
        /// </summary>
        /// <returns></returns>
        [DontWrapResult]
        [HttpPost]
        public async Task<string> Notify()
        {

#if NET35 || NET40 || NET45 || NET461
            ResponseHandler resHandler = new ResponseHandler(System.Web.HttpContext.Current);
#else
            ResponseHandler resHandler = new ResponseHandler( _httpContext.HttpContext);
#endif

            resHandler.SetKey(_configuration.TenPayKey);
            OrderQueryResult result = new OrderQueryResult(resHandler.ParseXML());

            //验证请求是否从微信发过来（安全）
            if (resHandler.IsTenpaySign())
            {
                if (result.IsReturnCodeSuccess())
                {
                    var jobArgs = _objectMapper.Map<ProcessWeChatPaymentJobArgs>(result);
                    await _backgroundJobManager.EnqueueAsync<ProcessWeChatPaymentJob, ProcessWeChatPaymentJobArgs>(jobArgs);
                }
                else
                {
                    _logger.Error(L("Payments.WeChat.PayFail", result.err_code, result.err_code_des));
                }
            }

            string xml = string.Format(@"<xml><return_code><![CDATA[{0}]]></return_code><return_msg><![CDATA[OK]]></return_msg></xml>", result.result_code);
            return await Task.FromResult(xml);
        }
    }
}
