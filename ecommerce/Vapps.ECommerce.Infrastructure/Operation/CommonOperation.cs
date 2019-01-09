using Abp.Dependency;
using Castle.Core.Logging;

namespace Vapps.WeChat.Operation
{
    /// <summary>
    /// 微信通用操作类
    /// </summary>
    public class CommonOperation : BaseOperation, ITransientDependency
    {
        private readonly ILogger _logger;

        public CommonOperation(ILogger logger,
            WeChatCommonHepler wechatCommonHepler)
        {
            this._logger = logger;
            var loginProviderSetting = wechatCommonHepler.GetLoginProviderSetting(WeChatConsts.MPNAME);
            Init(loginProviderSetting.AppId, loginProviderSetting.AppSecret);
        }
    }
}
