using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Advert.AdvertAccounts.Sync.Tenant
{
    public class TenantDailyAdvert
    {
        [JsonProperty("page_info")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("list")]
        public List<TenantDailyAdvertItem> Items { get; set; }
    }


    public class TenantDailyAdvertItem
    {
        [JsonProperty("date")]
        public DateTime Date { get; set; }

        /// <summary>
        /// 推广计划 id
        /// </summary>
        [JsonProperty("campaign_id")]
        public string CampaignId { get; set; }

        /// <summary>
        /// 广告组 id
        /// </summary>
        [JsonProperty("adgroup_id")]
        public string AdgrouId { get; set; }

        /// <summary>
        /// 曝光量
        /// </summary>
        [JsonProperty("impression")]
        public int Impression { get; set; }

        /// <summary>
        /// 点击量
        /// </summary>
        [JsonProperty("click")]
        public int Click { get; set; }

        /// <summary>
        /// 消耗，单位为分
        /// </summary>
        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        /// <summary>
        /// 转化
        /// </summary>
        [JsonProperty("conversion")]
        public int Conversion { get; set; }

        /// <summary>
        /// 广告 id，当 level='AD' 时显示
        /// </summary>
        [JsonProperty("ad_id")]
        public int AdId { get; set; }

        /// <summary>
        /// 下单量
        /// </summary>
        [JsonProperty("order_placement")]
        public int OrderPlacement { get; set; }
    }

    public class TenantAccountFunds
    {
        [JsonProperty("list")]
        public List<TenantAccountFundsItem> Items { get; set; }
    }


    public class TenantAccountFundsItem
    {
        [JsonProperty("fund_type")]
        public string FundType { get; set; }

        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        [JsonProperty("fund_status")]
        public string FundStatus { get; set; }

        [JsonProperty("realtime_cost")]
        public decimal RealtimeCost { get; set; }
    }
}
