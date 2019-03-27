using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Vapps.Advert.AdvertAccounts.Sync.Tenant
{
    public class TenantOrder
    {
        [JsonProperty("page_info")]
        public PageInfo PageInfo { get; set; }

        [JsonProperty("list")]
        public List<TenantOrderItem> Orders { get; set; }
    }

    public class TenantOrderItem
    {
        /// <summary>
        /// 广告组Id
        /// </summary>
        [JsonProperty("adgroup_id")]
        public string AdgroupId { get; set; }

        /// <summary>
        /// 广告组名称
        /// </summary>
        [JsonProperty("adgroup_name")]
        public string AdgroupName { get; set; }

        /// <summary>
        /// 广告账户Id
        /// </summary>
        [JsonProperty("from_account_id")]
        public string AccountId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        [JsonProperty("ecommerce_order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        [JsonProperty("quantity")]
        public int Quantity { get; set; }

        /// <summary>
        /// 价格
        /// </summary>
        [JsonProperty("price")]
        public int Price { get; set; }

        /// <summary>
        /// 订单总价格
        /// </summary>
        [JsonProperty("total_price")]
        public int TotalPrice { get; set; }

        /// <summary>
        /// 收件人名称
        /// </summary>
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        /// <summary>
        /// 收件人电话
        /// </summary>
        [JsonProperty("user_phone")]
        public string UserPhone { get; set; }

        /// <summary>
        /// 省份
        /// </summary>
        [JsonProperty("user_province")]
        public string UserProvince { get; set; }

        /// <summary>
        /// 城市
        /// </summary>
        [JsonProperty("user_city")]
        public string UserCity { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        [JsonProperty("user_area")]
        public string UserArea { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        [JsonProperty("user_address")]
        public string UserAddress { get; set; }

        /// <summary>
        /// 下单Ip
        /// </summary>
        [JsonProperty("user_ip")]
        public string UserIp { get; set; }

        /// <summary>
        /// 下单时间
        /// </summary>
        [JsonProperty("ecommerce_order_time")]
        public DateTime OrderTime { get; set; }

        /// <summary>
        /// 用户留言
        /// </summary>
        [JsonProperty("user_message")]
        public string UserMessage { get; set; }

        /// <summary>
        /// 下单页面
        /// </summary>
        [JsonProperty("destination_url")]
        public string Url { get; set; }

        /// <summary>
        /// 页面名称
        /// </summary>
        [JsonProperty("customized_page_name")]
        public string PageName { get; set; }

        /// <summary>
        /// 套餐详情
        /// </summary>
        [JsonProperty("commodity_package_detail")]
        public string PackageInfo { get; set; }
    }
    public class DateRangePara
    {
        /// <summary>
        /// 开始日期
        /// </summary>
        [JsonProperty("start_date")]
        public string StartDate { get; set; }

        /// <summary>
        /// 结束日期
        /// </summary>
        [JsonProperty("end_date")]
        public string EndDate { get; set; }
    }
}
