namespace Vapps.ECommerce.Payments
{
    /// <summary>
    /// 付款状态枚举
    /// </summary>
    public enum PaymentStatus : int
    {
        /// <summary>
        /// 未付款
        /// </summary>
        Pending = 10,

        /// <summary>
        /// 已支付
        /// </summary>
        Paid = 30,

        /// <summary>
        /// 部分退款
        /// </summary>
        PartiallyRefunded = 35,

        /// <summary>
        /// 已退款
        /// </summary>
        Refunded = 40,
    }
}
