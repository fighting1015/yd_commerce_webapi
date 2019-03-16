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
        NotRequired = 100,

        /// <summary>
        /// 未发货
        /// </summary>
        NotYetShipped = 200,

        /// <summary>
        /// 部分发货
        /// </summary>
        PartiallyShipped = 250,

        /// <summary>
        /// 已发货(待打单)
        /// </summary>
        Shipped = 300,

        /// <summary>
        /// 待取件
        /// </summary>
        NoTrace = 301,

        /// <summary>
        /// 已揽收
        /// </summary>
        Taked = 302,

        /// <summary>
        /// 在途
        /// </summary>
        OnPassag = 303,

        /// <summary>
        /// 到达派件城市
        /// </summary>
        DestinationCity = 304,

        /// <summary>
        /// 派件中
        /// </summary>
        Delivering = 305,

        /// <summary>
        /// 已签收
        /// </summary>
        Received = 306,

        /// <summary>
        /// 问题件
        /// </summary>
        Issue = 400,

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
