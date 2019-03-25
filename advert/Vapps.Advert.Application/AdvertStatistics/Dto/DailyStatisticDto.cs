using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Vapps.Advert.AdvertStatistics.Dto
{
    public class DailyStatisticDto : EntityDto<long>
    {
        public DailyStatisticDto()
        {
            this.Items = new List<DailyStatisticItemDto>();
        }

        /// <summary>
        /// 商品名Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        public string ProductName { get; set; }

        /// <summary>
        /// 广告账户Id
        /// </summary>
        public long AdvertAccountId { get; set; }

        /// <summary>
        /// 广告账户
        /// </summary>
        public string AdvertAccount { get; set; }

        /// <summary>
        /// 展现数
        /// </summary>
        public int DisplayNum { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        public int ClickNum { get; set; }

        /// <summary>
        /// 点击价格
        /// </summary>
        public decimal ClickPrice { get; set; }

        /// <summary>
        /// 点记录
        /// </summary>
        public decimal ClickRate { get; set; }

        /// <summary>
        /// 千次展现花费
        /// </summary>
        public decimal ThDisplayCost { get; set; }

        /// <summary>
        /// 消耗金额
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 统计时间
        /// </summary>
        public string StatisticOn { get; set; }

        /// <summary>
        /// 条目(每小时)
        /// </summary>
        public List<DailyStatisticItemDto> Items { get; set; }
    }

    public class DailyStatisticItemDto : EntityDto<long>
    {
        /// <summary>
        /// 小时（24小时制）
        /// </summary>
        public int HourOfDay { get; set; }

        /// <summary>
        /// 展示数量
        /// </summary>
        public int DisplayNum { get; set; }

        /// <summary>
        /// 点击数量
        /// </summary>
        public int ClickNum { get; set; }

        /// <summary>
        /// 点击价格
        /// </summary>
        public decimal ClickPrice { get; set; }

        /// <summary>
        /// 点记录
        /// </summary>
        public decimal ClickRate { get; set; }

        /// <summary>
        /// 千次展示费用
        /// </summary>
        public decimal ThDisplayCost { get; set; }

        /// <summary>
        /// 总消耗
        /// </summary>
        public decimal TotalCost { get; set; }
    }
}
