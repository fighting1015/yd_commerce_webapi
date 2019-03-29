using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using Vapps.ECommerce.Orders;

namespace Vapps.Statistic.ShipmentStatistics
{
    public class ShipmentStatistic : CreationAuditedEntity<long>, IMustHaveTenant, ISoftDelete
    {
        public int TenantId { get; set; }

        /// <summary>
        /// 渠道
        /// </summary>
        public OrderSource Channel { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 统计时间
        /// </summary>
        public DateTime Date { get; set; }

        /// <summary>
        /// 发货数量
        /// </summary>
        public int ShipmentNum { get; set; }

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
        /// 拒收数量
        /// </summary>
        public decimal RefusalTotal { get; set; }

        /// <summary>
        /// 在途数量
        /// </summary>
        public int OnPassagNum { get; set; }

        /// <summary>
        /// 同城数量
        /// </summary>
        public int DestinationCityNum { get; set; }

        /// <summary>
        /// 派送中数量
        /// </summary>
        public int DeliveringNum { get; set; }

        /// <summary>
        /// 问题件数量
        /// </summary>
        public int IssueNum { get; set; }

        /// <summary>
        /// 结算数量
        /// </summary>
        public int ClearNum { get; set; }

        /// <summary>
        /// 结算金额
        /// </summary>
        public decimal CleaTotal { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
