using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders.Toutiao.Requests
{
    public class OrderListRequestPara
    {
        public OrderListRequestPara()
        {
            this.Page = "0";
            this.Size = "100";
        }

        [JsonProperty("order_status")]
        public string OrderStatus { get; set; }

        [JsonProperty("start_time")]
        public string StartTime { get; set; }

        [JsonProperty("end_time")]
        public string EndTime { get; set; }

        [JsonProperty("order_by")]
        public string OrderBy { get; set; }

        [JsonProperty("is_desc")]
        public string IsDesc { get; set; }

        [JsonProperty("page")]
        public string Page { get; set; }

        [JsonProperty("size")]
        public string Size { get; set; }

    }
}
