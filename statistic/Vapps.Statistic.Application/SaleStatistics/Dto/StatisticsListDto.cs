using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Statistic.SaleStatistics.Dto
{
    public class StatisticsListDto 
    {
        /// <summary>
        /// 统计时间
        /// </summary>
        public string Date { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public string Product { get; set; }


        /// <summary>
        /// 下单数量
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// 下单金额
        /// </summary>
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// 广告消耗
        /// </summary>
        public decimal AdvertCost { get; set; }

        /// <summary>
        /// 转化成本
        /// </summary>
        public decimal TransformCost { get; set; }

        /// <summary>
        /// ROI
        /// </summary>
        public decimal Roi { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        public int ShipmentNum { get; set; }

        /// <summary>
        /// 签收数量
        /// </summary>
        public int ReceivedNum { get; set; }

        /// <summary>
        /// 签收金额
        /// </summary>
        public decimal ReceivedTotal { get; set; }

        /// <summary>
        /// 签收率
        /// </summary>
        public decimal ReceivedRate { get; set; }

        /// <summary>
        /// 签收成本
        /// </summary>
        public decimal ReceivedCost { get; set; }

        /// <summary>
        /// 拒签数量
        /// </summary>
        public int RejectNum { get; set; }

        /// <summary>
        /// 拒签金额
        /// </summary>
        public decimal RejectTotal { get; set; }

        /// <summary>
        /// 拒签率
        /// </summary>
        public decimal RejectRate { get; set; }

        /// <summary>
        /// 货物成本
        /// </summary>
        public decimal GoodsCost { get; set; }

        /// <summary>
        /// 物流费用
        /// </summary>
        public decimal ShipmentCost { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal RewardCost { get; set; }

        /// <summary>
        /// 总成本
        /// </summary>
        public decimal TotalCost { get; set; }

        /// <summary>
        /// 利润
        /// </summary>
        public decimal Profit { get; set; }

        /// <summary>
        /// 利润率
        /// </summary>
        public decimal ProfitRate { get; set; }

        /// <summary>
        /// 成本利润率
        /// </summary>
        public decimal CostProfitRate { get; set; }
    }
}
