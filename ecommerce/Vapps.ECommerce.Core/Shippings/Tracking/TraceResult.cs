using System.Collections.Generic;

namespace Vapps.ECommerce.Shippings.Tracking
{
    public class TraceResult
    {
        public TraceResult()
        {
            this.Traces = new List<TracesItem>();
        }

        public long OrderId { get; set; }

        public string LogisticCode { get; set; }

        public string ShipperCode { get; set; }

        public int State { get; set; }

        public string EBusinessId { get; set; }

        public string Reason { get; set; }

        public bool Success { get; set; }

        public string UpdateTime { get; set; }

        public List<TracesItem> Traces { get; set; }
    }

    public class TracesItem
    {
        public string AcceptStation { get; set; }

        public string AcceptTime { get; set; }

        public string Remark { get; set; }
    }
}
