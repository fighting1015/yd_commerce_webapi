using Newtonsoft.Json;

namespace Vapps.ECommerce.Orders.Toutiao.Requests
{
    public class OrderLogisticsAddRequestPara
    {
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        [JsonProperty("logistics_id")]
        public string LogisticsId { get; set; }

        [JsonProperty("logistics_code")]
        public string LogisticsCode { get; set; }
    }
}
