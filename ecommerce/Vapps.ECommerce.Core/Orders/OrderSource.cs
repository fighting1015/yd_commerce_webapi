namespace Vapps.ECommerce.Orders
{
    public enum OrderSource
    {
        /// <summary>
        /// 自建
        /// </summary>
        Self = 10,

        /// <summary>
        /// 放心购广告
        /// </summary>
        FxgAd = 20,

        /// <summary>
        /// 放心购频道
        /// </summary>
        FxgPd = 30,

        /// <summary>
        /// 广点通
        /// </summary>
        Tenant = 40,

        /// <summary>
        /// 有赞
        /// </summary>
        YouZan = 50,
    }

    public enum ToutiaoOrderSource
    {
        /// <summary>
        /// 付费-广告
        /// 佣金率:0%
        /// </summary>
        Charge_Ad = 1,

        /// <summary>
        /// 付费-广告
        /// 佣金率:10%
        /// </summary>
        Charge_Other = 2,

        /// <summary>
        /// 付费-自媒体
        /// 佣金率:商品设置为准
        /// </summary>
        Charge_Media = 4,

        /// <summary>
        /// 付费-放心购频道
        /// 佣金率:10%
        /// </summary>
        Charge_Fxg = 5,

        /// <summary>
        /// 免费-后台创建
        /// 佣金率:0%
        /// </summary>
        Free_Create = 8,

        /// <summary>
        /// 免费-其他平台自费推广
        /// 佣金率:0%
        /// </summary>
        Free_OtherPlat = 9,

        /// <summary>
        /// 免费-其他
        /// 佣金率:0%
        /// </summary>
        Free_Other = 10,

        /// <summary>
        /// 免费-微头条
        /// 佣金率:0%
        /// </summary>
        Free_MicroHeadband = 11,
    }
}
