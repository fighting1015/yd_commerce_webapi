using Newtonsoft.Json;

namespace Vapps.Helpers
{
    public class RequestAnalysisDataResult
    {
        //
        // 摘要: code
        //     Gets or sets the code.
        [JsonProperty("code")]
        public int Code { get; set; }

        //
        // 摘要: code
        //     Gets or sets the code.
        //[JsonIgnore]
        //public string data { get; set; }
    }

    public class Ip2AddressEntity
    {
        public Ip2AddressEntity()
        {
            AddressData = new AddressData();
        }

        //
        // 摘要: code
        //     Gets or sets the code.
        [JsonProperty("code")]
        public int Code { get; set; }

        //
        // 摘要: code
        //     Gets or sets the code.
        [JsonProperty("data")]
        public AddressData AddressData { get; set; }
    }

    public class AddressData
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_id")]
        public string CountryCode { get; set; }

        /// <summary>
        /// 地区:如华南
        /// </summary>
        [JsonProperty("area")]
        public string Area { get; set; }

        /// <summary>
        /// 地区Id
        /// </summary>
        [JsonProperty("area_id")]
        public string AreaId { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [JsonProperty("region")]
        public string Region { get; set; }

        /// <summary>
        /// 省份Id
        /// </summary>
        [JsonProperty("region_id")]
        public string RegionId { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [JsonProperty("city")]
        public string City { get; set; }

        /// <summary>
        /// 城市Id
        /// </summary>
        [JsonProperty("city_id")]
        public string CityId { get; set; }

        /// <summary>
        /// 镇
        /// </summary>
        [JsonProperty("county")]
        public string County { get; set; }

        /// <summary>
        /// 镇Id
        /// </summary>
        [JsonProperty("county_id")]
        public string CountyId { get; set; }

        /// <summary>
        /// 运营商
        /// </summary>
        [JsonProperty("isp")]
        public string Isp { get; set; }

        /// <summary>
        /// 运营商id
        /// </summary>
        [JsonProperty("isp_id")]
        public string IspId { get; set; }
    }
}
