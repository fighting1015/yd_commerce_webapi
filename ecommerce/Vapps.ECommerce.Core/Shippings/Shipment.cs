using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.ECommerce.Orders;

namespace Vapps.ECommerce.Shippings
{
    /// <summary>
    /// 物流单
    /// </summary>
    [Table("Shipments")]
    public partial class Shipment : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual long OrderId { get; set; }

        /// <summary>
        /// 订单
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public virtual string OrderNumber { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public virtual ShippingStatus Status { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal TotalWeight { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        public virtual decimal TotalVolume { get; set; }

        /// <summary>
        /// 拒签时间
        /// </summary>
        public virtual DateTime? RejectedOn { get; set; }

        /// <summary>
        /// 签收时间
        /// </summary>
        public virtual DateTime? ReceivedOn { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public virtual string AdminComment { get; set; }

        /// <summary>
        /// 快递Id(非租户绑定的快递Id)
        /// </summary>
        public virtual int? LogisticsId { get; set; }

        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string LogisticsName { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public virtual string LogisticsNumber { get; set; }

        /// <summary>
        /// 快递详情信息（格式化物流跟踪信息）
        /// </summary>
        public virtual string ShipmentDetail { get; set; }

        /// <summary>
        /// Gets or sets the shipment items
        /// </summary>
        [ForeignKey("ShipmentId")]
        public virtual ICollection<ShipmentItem> Items { get; set; }


        public bool IsShiped()
        {
            if (Status == ShippingStatus.NotYetShipped)
                return false;
            else if (Status == ShippingStatus.NotRequired)
                return false;

            return true;
        }
    }
}
