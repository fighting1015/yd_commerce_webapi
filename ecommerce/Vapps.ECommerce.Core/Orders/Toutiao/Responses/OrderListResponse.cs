using Newtonsoft.Json;
using System.Collections.Generic;

namespace Vapps.ECommerce.Orders.Toutiao.Responses
{
    public class OrderListResponse
    {
        public OrderListResponse()
        {
            this.Items = new List<OrderListItem>();
        }

        [JsonProperty("count")]
        public int Count { get; set; }

        [JsonProperty("total")]
        public int Total { get; set; }

        /// <summary>
        /// 订单列表
        /// </summary>
        [JsonProperty("list")]
        public List<OrderListItem> Items { get; set; }
    }

    public class OrderListItem
    {
        [JsonProperty("buyer_words")]
        public string BuyerWords { get; set; }

        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }

        /// <summary>
        /// 
        /// </summary>
        [JsonProperty("child")]
        public List<OrderChildItem> Childs { get; set; }

        /// <summary>
        /// 子订单数量
        /// </summary>
        [JsonProperty("child_num")]
        public int ChildNum { get; set; }

        /// <summary>
        /// 平台优惠券金额 (单位: 分)
        /// </summary>
        [JsonProperty("coupon_amount")]
        public int CouponAmount { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        [JsonProperty("create_time")]
        public string CreateTime { get; set; }

        /// <summary>
        /// 订单最后状态
        /// </summary>
        [JsonProperty("final_status")]
        public int FinalStatus { get; set; }

        /// <summary>
        /// 是否评价 (1:已评价)
        /// </summary>
        [JsonProperty("is_comment")]
        public int IsComment { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        [JsonProperty("logistics_code")]
        public string LogisticsCode { get; set; }

        /// <summary>
        /// 物流公司id
        /// </summary>
        [JsonProperty("logistics_id")]
        public int LogisticsId { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        [JsonProperty("logistics_time")]
        public string LogisticsTime { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        [JsonProperty("order_status")]
        public int OrderStatus { get; set; }

        /// <summary>
        /// 父订单总金额 (单位: 分) 即用户实际支付金额, 不包含运费
        /// </summary>
        [JsonProperty("order_total_amount")]
        public int OrderTotalAmount { get; set; }

        /// <summary>
        /// 支付时间 (pay_type为0货到付款时, 此字段为空)
        /// </summary>
        [JsonProperty("pay_time")]
        public string PayTime { get; set; }

        /// <summary>
        /// 支付类型 (0：货到付款，1：微信，2：支付宝)
        /// </summary>
        [JsonProperty("pay_type")]
        public string PayType { get; set; }

        [JsonProperty("post_addr")]
        public PostAddress PostAddress { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        [JsonProperty("post_amount")]
        public string PostAmount { get; set; }

        /// <summary>
        /// post_code
        /// </summary>
        [JsonProperty("post_code")]
        public string PostCode { get; set; }

        /// <summary>
        /// 收件人姓名
        /// </summary>
        [JsonProperty("post_receiver")]
        public string PostReceiver { get; set; }

        /// <summary>
        /// 收件人电话
        /// </summary>
        [JsonProperty("post_tel")]
        public string PostTel { get; set; }

        /// <summary>
        /// 收货时间
        /// </summary>
        [JsonProperty("receipt_time")]
        public string ReceiptTime { get; set; }

        /// <summary>
        /// 商家留言
        /// </summary>
        [JsonProperty("seller_words")]
        public string SellerWords { get; set; }

        /// <summary>
        /// 商家优惠券金额 (单位: 分)
        /// </summary>
        [JsonProperty("shop_coupon_amount")]
        public string ShopCouponAmount { get; set; }

        /// <summary>
        /// 订单所属商户id
        /// </summary>
        [JsonProperty("shop_id")]
        public string ShopId { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("update_time")]
        public string UpdateTime { get; set; }

        /// <summary>
        /// 催单次数
        /// </summary>
        [JsonProperty("urge_cnt")]
        public string UrgeCnt { get; set; }

        /// <summary>
        /// 买家用户名 (暂时为空)
        /// </summary>
        [JsonProperty("user_name")]
        public string UserName { get; set; }


    }

    public class PostAddress
    {
        [JsonProperty("province")]
        public AddressItem Provice { get; set; }

        [JsonProperty("city")]
        public AddressItem City { get; set; }

        [JsonProperty("town")]
        public AddressItem Town { get; set; }

        [JsonProperty("detail")]
        public string Detail { get; set; }

        #region Nested classes

        public class AddressItem
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

        }

        #endregion
    }


    public class OrderChildItem
    {
        [JsonProperty("buyer_words")]
        public string BuyerWords { get; set; }

        /// <summary>
        /// 活动id
        /// </summary>
        [JsonProperty("campaign_id")]
        public string CampaignId { get; set; }

        /// <summary>
        /// 活动细则 (活动可能会导致商品成交价非combo_amount, 如有活动, 以活动的sale_price为准)
        /// </summary>
        [JsonProperty("campaign_info")]
        public List<CampaignInfo> CampaignInfos { get; set; }

        [JsonProperty("cancel_reason")]
        public string CancelReason { get; set; }

        /// <summary>
        /// 该子订单所购买的sku的售价
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// 该子订单所购买的sku的售价
        /// </summary>
        [JsonProperty("combo_amount")]
        public int ComboAmount { get; set; }

        /// <summary>
        /// 该子订单所购买的sku的数量
        /// </summary>
        [JsonProperty("combo_num")]
        public int ComboNum { get; set; }

        /// <summary>
        /// 平台优惠券金额 (单位: 分)
        /// </summary>
        [JsonProperty("coupon_amount")]
        public int CouponAmount { get; set; }

        /// <summary>
        /// 平台优惠券金额 (单位: 分)
        /// </summary>
        [JsonProperty("coupon_info")]
        public List<CouponInfo> CouponInfos { get; set; }

        [JsonProperty("coupon_meta_id")]
        public string CouponMetaId { get; set; }

        /// <summary>
        /// 订单创建时间
        /// </summary>
        [JsonProperty("create_time")]
        public string CreateTime { get; set; }

        /// <summary>
        /// 订单最后状态
        /// </summary>
        [JsonProperty("final_status")]
        public int FinalStatus { get; set; }

        /// <summary>
        /// 是否评价 (1:已评价)
        /// </summary>
        [JsonProperty("is_comment")]
        public string IsComment { get; set; }

        /// <summary>
        /// 运单号
        /// </summary>
        [JsonProperty("logistics_code")]
        public string LogisticsCode { get; set; }

        /// <summary>
        /// 物流公司id
        /// </summary>
        [JsonProperty("logistics_id")]
        public int LogisticsId { get; set; }

        /// <summary>
        /// 发货时间
        /// </summary>
        [JsonProperty("logistics_time")]
        public string LogisticsTime { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        [JsonProperty("order_id")]
        public string OrderId { get; set; }

        /// <summary>
        /// 订单id
        /// </summary>
        [JsonProperty("order_status")]
        public ToutiaoOrderStatus OrderStatus { get; set; }

        /// <summary>
        /// 该子订单购买的商品外部id (如接入方添加商品时表示id为该字段, 则以该id为准)
        /// </summary>
        [JsonProperty("out_product_id")]
        public int OutProductId { get; set; }

        /// <summary>
        /// 该子订单购买的商品外部 sku_id (如接入方添加sku时表示id为该字段, 则以该id为准)
        /// </summary>
        [JsonProperty("out_sku_id")]
        public int OutSkuId { get; set; }

        /// <summary>
        /// 支付时间 (pay_type为0货到付款时, 此字段为空)
        /// </summary>
        [JsonProperty("pay_time")]
        public string PayTime { get; set; }

        /// <summary>
        /// 支付类型 (0：货到付款，1：微信，2：支付宝)
        /// </summary>
        [JsonProperty("pay_type")]
        public int PayType { get; set; }

        /// <summary>
        /// 父订单id
        /// </summary>
        [JsonProperty("pid")]
        public string PId { get; set; }

        [JsonProperty("post_addr")]
        public PostAddress PostAddress { get; set; }

        /// <summary>
        /// 邮费
        /// </summary>
        [JsonProperty("post_amount")]
        public int PostAmount { get; set; }

        /// <summary>
        /// post_code
        /// </summary>
        [JsonProperty("post_code")]
        public string PostCode { get; set; }

        /// <summary>
        /// 收件人姓名
        /// </summary>
        [JsonProperty("post_receiver")]
        public string PostReceiver { get; set; }

        /// <summary>
        /// 收件人电话
        /// </summary>
        [JsonProperty("post_tel")]
        public string PostTel { get; set; }

        /// <summary>
        /// 该子订单购买的商品id
        /// </summary>
        [JsonProperty("product_id")]
        public string ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        [JsonProperty("product_name")]
        public string ProductName { get; set; }

        /// <summary>
        /// 收货时间
        /// </summary>
        [JsonProperty("receipt_time")]
        public string ReceiptTime { get; set; }

        /// <summary>
        /// 商家留言
        /// </summary>
        [JsonProperty("seller_words")]
        public string SellerWords { get; set; }

        /// <summary>
        /// 商家优惠券金额 (单位: 分)
        /// </summary>
        [JsonProperty("shop_coupon_amount")]
        public int ShopCouponAmount { get; set; }

        /// <summary>
        /// 订单所属商户id
        /// </summary>
        [JsonProperty("shop_id")]
        public int ShopId { get; set; }

        /// <summary>
        /// 该子订单所属商品规格描述
        /// </summary>
        [JsonProperty("spec_desc")]
        public List<Spec> SpecDesc { get; set; }

        /// <summary>
        /// 该子订单总金额 (单位: 分)
        /// </summary>
        [JsonProperty("total_amount")]
        public string TotalAmount { get; set; }

        /// <summary>
        /// 更新时间
        /// </summary>
        [JsonProperty("update_time")]
        public string UpdateTime { get; set; }

        /// <summary>
        /// 催单次数
        /// </summary>
        [JsonProperty("urge_cnt")]
        public string UrgeCnt { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        [JsonProperty("c_type")]
        public ToutiaoOrderSource OrderSource { get; set; }

        /// <summary>
        /// 佣金比率
        /// </summary>
        [JsonProperty("cos_ratio")]
        public decimal CosRatio { get; set; }

        /// <summary>
        /// 买家用户名 (暂时为空)
        /// </summary>
        [JsonProperty("user_name")]
        public string UserName { get; set; }

        public class Spec
        {
            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("value")]
            public string value { get; set; }
        }

        public class CouponInfo
        {
            [JsonProperty("id")]
            public string Id { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("description")]
            public string Description { get; set; }

            [JsonProperty("credit")]
            public int Credit { get; set; }

            [JsonProperty("type")]
            public int Type { get; set; }

            [JsonProperty("discount")]
            public int Discount { get; set; }

            [JsonProperty("pay_type")]
            public int PayType { get; set; }
        }

        public class CampaignInfo
        {
            [JsonProperty("campaign_id")]
            public string Id { get; set; }

            [JsonProperty("title")]
            public string Title { get; set; }

            [JsonProperty("sale_price")]
            public string SalePrice { get; set; }

            [JsonProperty("market_price")]
            public int MarketPrice { get; set; }
        }

    }
}
