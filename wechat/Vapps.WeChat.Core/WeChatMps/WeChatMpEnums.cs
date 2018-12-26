namespace Vapps.WeChat.Core.WeChatMps
{
    /// <summary>
    /// 授权公众号
    /// </summary>
    public enum ServiceTypeInfo
    {
        /// <summary>
        /// 订阅号
        /// </summary>
        Subscription = 0,

        /// <summary>
        /// 由历史老帐号升级后的订阅号
        /// </summary>
        OldSubscription = 1,

        /// <summary>
        /// 代表服务号
        /// </summary>
        Service = 2
    }

    /// <summary>
    /// 授权方认证类型
    /// </summary>
    public enum VerifyType
    {
        /// <summary>
        /// -1代表未认证
        /// </summary>
        UnVerify = -1,

        /// <summary>
        /// 0代表微信认证
        /// </summary>
        WeChatVerify = 0,

        /// <summary>
        /// 1代表新浪微博认证
        /// </summary>
        SinaWeiBoVerify = 1,

        /// <summary>
        /// 2代表腾讯微博认证
        /// </summary>
        TencentWeiBoVerify = 2,

        /// <summary>
        /// 3代表已资质认证通过但还未通过名称认证
        /// </summary>
        QualityVerifyButNotNameVerify = 3,

        /// <summary>
        /// 4代表已资质认证通过、还未通过名称认证，但通过了新浪微博认证
        /// </summary>
        QualityVerifyAndSinaWeiBoVerify = 4,

        /// <summary>
        /// 5代表已资质认证通过、还未通过名称认证，但通过了腾讯微博认证
        /// </summary>
        QualityVerifyAndTencentWeiBoVerify = 4,
    }
}
