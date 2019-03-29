using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using Vapps.ECommerce.Orders;

namespace Vapps.Statistic.SaleStatistics
{
    public class SaleStatistic : CreationAuditedEntity<long>, IMustHaveTenant, ISoftDelete
    {
        public int TenantId { get; set; }

        /// <summary>
        /// 渠道Id
        /// </summary>
        public OrderSource Channel { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 订单数量
        /// </summary>
        public int OrderNum { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        public int ShipmentNum { get; set; }

        /// <summary>
        /// 销售额
        /// </summary>
        public decimal OrderTotal { get; set; }

        /// <summary>
        /// 广告消耗
        /// </summary>
        public decimal AdvertCost { get; set; }

        /// <summary>
        /// 签收数量
        /// </summary>
        public int DeliverNum { get; set; }

        /// <summary>
        /// 签收金额
        /// </summary>
        public decimal DeliverTotal { get; set; }

        /// <summary>
        /// 拒收数量
        /// </summary>
        public int RefusalNum { get; set; }

        /// <summary>
        /// 拒收金额
        /// </summary>
        public decimal RefusalTotal { get; set; }

        /// <summary>
        /// 货物成本
        /// </summary>
        public decimal GoodsCost { get; set; }

        /// <summary>
        /// 物流成本
        /// </summary>
        public decimal ShipmentCost { get; set; }

        /// <summary>
        /// 佣金
        /// </summary>
        public decimal RewardAmount { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
