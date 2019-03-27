using System;

namespace Vapps.Advert.AdvertStatistics
{
    public class AdvertStatisticImport
    {
        /// <summary>
        /// 商品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 广告账户Id
        /// </summary>
        public long AdvertAccountId { get; set; }

        /// <summary>
        /// 展示数
        /// </summary>
        public int DisplayNum { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        public int ClickNum { get; set; }

        /// <summary>
        /// 平均点击价格
        /// </summary>
        public decimal ClickPrice { get; set; }

        /// <summary>
        /// 平均千次展现费用
        /// </summary>
        public decimal ThDisplayCost { get; set; }

        /// <summary>
        /// 总花费
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 统计日期
        /// </summary>
        public DateTime StatisticOnUtc { get; set; }
    }
}
