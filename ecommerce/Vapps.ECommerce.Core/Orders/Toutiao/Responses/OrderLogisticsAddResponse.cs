using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders.Toutiao.Responses
{
    public class OrderLogisticsAddResponse
    {
        [JsonProperty("st")]
        public int ST { get; set; }

        [JsonProperty("msg")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public string[] Data { get; set; }
    }
}
