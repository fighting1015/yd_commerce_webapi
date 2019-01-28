using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

using System.Collections.Generic;

namespace Vapps.ECommerce.Shipping
{
    /// <summary>
    /// 发货表
    /// </summary>
    public partial class Shipment : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public int OrderId { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public string TrackingNumber { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public decimal? TotalWeight { get; set; }

        /// <summary>
        /// 签收时间
        /// </summary>
        public DateTime? DeliveryDateUtc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string AdminComment { get; set; }

        /// <summary>
        /// 快递Id
        /// </summary>
        public int? ShipInfoId { get; set; }

        /// <summary>
        /// 快递详情信息
        /// </summary>
        public string ShipmentDetail { get; set; }

        /// <summary>
        /// 运费是否已结算
        /// </summary>
        public bool IsCleared { get; set; }

        /// <summary>
        /// Gets or sets the shipment items
        /// </summary>
        public virtual ICollection<ShipmentItem> ShipmentItems { get; set; }
    }
}
