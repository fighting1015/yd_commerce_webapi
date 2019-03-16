namespace Vapps.ECommerce.Shippings.Dto.Tracking
{
    public class GetShipmentTrackingInput
    {
        /// <summary>
        /// 订单Id(订单/物流Id二传一)
        /// </summary>
        public long? OrderId { get; set; }

        /// <summary>
        /// 发货记录Id(订单/物流Id二传一)
        /// </summary>
        public long? ShipmentId { get; set; }

        /// <summary>
        /// 强制刷新(请求第三方)
        /// </summary>
        public bool Refresh { get; set; }
    }
}
