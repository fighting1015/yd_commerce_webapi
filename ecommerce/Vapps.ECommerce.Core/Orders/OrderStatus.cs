namespace Vapps.ECommerce.Orders
{
    /// <summary>
    /// 订单状态枚举
    /// </summary>
    public enum OrderStatus : int
    {
        /// <summary>
        /// 待确认
        /// </summary>
        WaitConfirm = 10,

        /// <summary>
        /// 处理中
        /// </summary>
        Processing = 20,

        /// <summary>
        /// 已完成
        /// </summary>
        Completed = 30,

        /// <summary>
        /// 已取消
        /// </summary>
        Canceled = 40
    }
}
