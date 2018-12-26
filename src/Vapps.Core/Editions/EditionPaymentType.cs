namespace Vapps.Editions
{
    public enum EditionPaymentType
    {
        /// <summary>
        /// 付费注册
        /// </summary>
        NewRegistration = 0,

        /// <summary>
        /// 现有租户购买，目前正在使用付费版本的试用版
        /// </summary>
        BuyNow = 1,

        /// <summary>
        /// 租户升级版本 (从免费版或低版本).
        /// </summary>
        Upgrade = 2,

        /// <summary>
        /// 版本到期时间延长(不会改变目前版本)
        /// </summary>
        Extend = 3
    }
}
