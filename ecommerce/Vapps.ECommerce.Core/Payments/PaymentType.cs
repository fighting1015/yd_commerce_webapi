namespace Vapps.ECommerce.Payments
{
    public enum PaymentType
    {
        /// <summary>
        /// 所有
        /// </summary>
        All = 0,

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
