namespace Vapps.ECommerce.Shippings.Dto.Shipments
{
    public class QuickDeliveryInput
    {
        /// <summary>
        /// 订单Id
        /// </summary>
        public long OrderId { get; set; }

        /// <summary>
        /// 租户自选物流Id
        /// </summary>
        public int LogisticsId { get; set; }

        /// <summary>
        /// 物流单号
        /// </summary>
        public string LogisticsNumber { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string AdminComment { get; set; }
    }
}
