namespace Vapps.ECommerce.Shippings
{
    /// <summary>
    /// 发货事件
    /// </summary>
    public class ShipmentSentEvent
    {
        private readonly Shipment _shipment;

        public ShipmentSentEvent(Shipment shipment)
        {
            this._shipment = shipment;
        }

        public Shipment Shipment
        {
            get { return _shipment; }
        }
    }

    /// <summary>
    /// 签收事件
    /// </summary>
    public class ShipmentDeliveredEvent
    {
        private readonly Shipment _shipment;

        public ShipmentDeliveredEvent(Shipment shipment)
        {
            this._shipment = shipment;
        }

        public Shipment Shipment
        {
            get { return _shipment; }
        }
    }

    /// <summary>
    /// 拒签事件
    /// </summary>
    public class ShipmentRejectedEvent
    {
        private readonly Shipment _shipment;

        public ShipmentRejectedEvent(Shipment shipment)
        {
            this._shipment = shipment;
        }

        public Shipment Shipment
        {
            get { return _shipment; }
        }
    }
}
