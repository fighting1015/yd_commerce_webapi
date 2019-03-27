using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Advert.AdvertAccounts.Sync.Toutiao
{
    public class ToutiaoAdvertResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("data")]
        public T Data { get; set; }
    }

    public class ToutiaoAccesstokenResponse
    {
        [JsonProperty("advertiser_id")]
        public string AdvertiserId { get; set; }

        [JsonProperty("access_token")]
        public string AccessToken { get; set; }

        [JsonProperty("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonProperty("expires_in")]
        public int AccessTokenExpiresIn { get; set; }

        [JsonProperty("refresh_token_expires_in")]
        public int RefreshTokenExpiresIn { get; set; }
    }

    /// <summary>
    /// 查询广告主余额
    /// </summary>
    public class ToutiaoFundResponse
    {
        /// <summary>
        /// 广告主ID
        /// </summary>
        [JsonProperty("advertiser_id")]
        public string AdvertiserId { get; set; }

        /// <summary>
        /// 账户名
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// 联系邮箱
        /// </summary>
        [JsonProperty("email")]
        public string Email { get; set; }

        /// <summary>
        /// 账户总余额(单位元)
        /// </summary>
        [JsonProperty("balance")]
        public decimal Balance { get; set; }

        /// <summary>
        /// 账户可用总余额(单位元)
        /// </summary>
        [JsonProperty("valid_balance")]
        public decimal ValidBalance { get; set; }

        /// <summary>
        /// 现金余额(单位元)
        /// </summary>
        [JsonProperty("cash")]
        public decimal Cash { get; set; }

        /// <summary>
        /// 现金可用余额(单位元)
        /// </summary>
        [JsonProperty("valid_cash")]
        public decimal ValidCash { get; set; }

        /// <summary>
        /// 赠款余额(单位元)
        /// </summary>
        [JsonProperty("grant")]
        public decimal Grant { get; set; }

        /// <summary>
        /// 赠款可用余额(单位元)
        /// </summary>
        [JsonProperty("valid_grant")]
        public decimal ValidGrant { get; set; }

    }

    public class ToutiaoDailyReportResponse
    {
        [JsonProperty("advertiser_id")]
        public string AdvertiserId { get; set; }

        [JsonProperty("cost")]
        public decimal Cost { get; set; }

        [JsonProperty("click")]
        public int Click { get; set; }

        [JsonProperty("show")]
        public int Show { get; set; }

        [JsonProperty("stat_datetime")]
        public DateTime StatDatetime { get; set; }
    }

    public class ToutiaoDailyReportListResponse
    {
        [JsonProperty("list")]
        public List<ToutiaoDailyReportResponse> List { get; set; }
    }
}
