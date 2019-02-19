namespace Vapps.ECommerce.Orders
{
    /// <summary>
    /// 订单类型
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 在线支付
        /// </summary>
        PayOnline = 1,

        /// <summary>
        /// 货到付款
        /// </summary>
        PayOnDelivery = 2,

        ///// <summary>
        ///// 积分商城兑换
        ///// </summary>
        //ConvertPoint = 3,
    }
}
