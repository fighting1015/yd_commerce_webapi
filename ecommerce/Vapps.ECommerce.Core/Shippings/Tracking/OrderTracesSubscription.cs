namespace Vapps.ECommerce.Shippings.Tracking
{
    public class OrderTracesSubscription
    {
        public OrderTracesSubscription()
        {
            this.Receiver = new SenderOrReceiver();
            this.Sender = new SenderOrReceiver();
        }

        public string CallBack { get; set; }
        public string OrderCode { get; set; }

        public string ShipperCode { get; set; }
        public string LogisticCode { get; set; }

        public SenderOrReceiver Receiver { get; set; }

        public SenderOrReceiver Sender { get; set; }
    }

    public class SenderOrReceiver
    {
        public string Company { get; set; }
        public string Name { get; set; }
        public string Tel { get; set; }
        public string Mobile { get; set; }
        public string PostCode { get; set; }
        public string ProvinceName { get; set; }
        public string CityName { get; set; }
        public string ExpAreaName { get; set; }
        public string Address { get; set; }
    }
}
