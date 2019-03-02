namespace Vapps.ECommerce.Shippings
{
    /// <summary>
    /// 发货状态枚举
    /// </summary>
    public enum ShippingStatus : int
    {
        /// <summary>
        /// 不需要发货
        /// </summary>
        ShippingNotRequired = 10,

        /// <summary>
        /// 未发货
        /// </summary>
        NotYetShipped = 20,

        /// <summary>
        /// 部分发货
        /// </summary>
        PartiallyShipped = 25,

        /// <summary>
        /// 待取件
        /// </summary>
        NoTrace = 0,

        /// <summary>
        /// 已揽收
        /// </summary>
        Taked = 1,

        /// <summary>
        /// 在途
        /// </summary>
        OnPassag = 2,

        /// <summary>
        /// 到达派件城市
        /// </summary>
        DestinationCity = 201,

        /// <summary>
        /// 派件中
        /// </summary>
        Delivering = 202,

        /// <summary>
        /// 已签收
        /// </summary>
        Received = 3,

        /// <summary>
        /// 问题件
        /// </summary>
        Issue = 4,

        /// <summary>
        /// 拒收(退件)
        /// </summary>
        IssueWithReject = 404,

        /// <summary>
        /// 取消
        /// </summary>
        Cancel = 500,

        /// <summary>
        /// 拦截
        /// </summary>
        Intercept = 600,
    }
}
