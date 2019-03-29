using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Statistic.ShipmentStatistics.Dto
{


    public class ShipmentStatisticDto
    {
        /// <summary>
        /// 渠道
        /// </summary>
        public string Channel { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// 快递
        /// </summary>
        public string Logistics { get; set; }

        /// <summary>
        /// 统计/下单时间
        /// </summary>
        public string Date { get; set; }

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
        /// 在途数量
        /// </summary>
        public int OnPassagNum { get; set; }

        /// <summary>
        /// 同城数量
        /// </summary>
        public int DestinationCityNum { get; set; }

        /// <summary>
        /// 派件数量
        /// </summary>
        public int DeliveringNum { get; set; }

        /// <summary>
        /// 问题件数量
        /// </summary>
        public int IssueNum { get; set; }

        /// <summary>
        /// 回款数量
        /// </summary>
        public int ClearNum { get; set; }

        /// <summary>
        /// 回款金额
        /// </summary>
        public decimal CleaTotal { get; set; }

        /// <summary>
        /// 回款率
        /// </summary>
        public decimal CleaRate { get; set; }

    }
}
