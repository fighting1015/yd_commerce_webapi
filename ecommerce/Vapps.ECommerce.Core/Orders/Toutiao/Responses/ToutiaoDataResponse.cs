using Newtonsoft.Json;

namespace Vapps.ECommerce.Orders.Toutiao.Responses
{
    public class ToutiaoDataResponse<T>
    {
        public ToutiaoDataResponse()
        {
            this.Data = System.Activator.CreateInstance<T>();
        }

        [JsonProperty("data")]
        public T Data { get; set; }

        [JsonProperty("err_no")]
        public int ErrorNo { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
