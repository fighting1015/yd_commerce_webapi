using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

using System.Collections.Generic;

namespace Vapps.ECommerce.Shipping
{
    /// <summary>
    /// 物流单
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
        public virtual int OrderId { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public virtual string TrackingNumber { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal? TotalWeight { get; set; }

        /// <summary>
        /// 签收时间
        /// </summary>
        public virtual DateTime? DeliveryDateUtc { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string AdminComment { get; set; }

        /// <summary>
        /// 快递Id
        /// </summary>
        public virtual int? LogisticsId { get; set; }

        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string LogisticsName { get; set; }

        /// <summary>
        /// 快递详情信息（格式化物流跟踪信息）
        /// </summary>
        public virtual string ShipmentDetail { get; set; }

        /// <summary>
        /// 运费是否已结算
        /// </summary>
        public virtual bool IsCleared { get; set; }

        /// <summary>
        /// Gets or sets the shipment items
        /// </summary>
        public virtual ICollection<ShipmentItem> ShipmentItems { get; set; }
    }
}
