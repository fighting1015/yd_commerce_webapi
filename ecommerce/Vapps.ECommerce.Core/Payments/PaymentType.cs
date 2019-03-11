namespace Vapps.ECommerce.Payments
{
    public enum PaymentType
    {
        /// <summary>
        /// 在线支付
        /// </summary>
        PayOnline = 1,

        /// <summary>
        /// 货到付款
        /// </summary>
        PayOnDelivery = 2,
    }

    /// <summary>
    /// 支付方法
    /// </summary>
    public enum PaymentMethod
    {
        /// <summary>
        /// 支付宝
        /// </summary>
        AliPay = 1,

        /// <summary>
        /// 微信
        /// </summary>
        WeChat = 2,

        /// <summary>
        /// 货到付款
        /// </summary>
        PayOnDelivery = 3,
    }
}
