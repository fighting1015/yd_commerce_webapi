﻿using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Orders
{
    /// <summary>
    /// Represents an order item
    /// </summary>
    [Table("OrderItems")]
    public partial class OrderItem : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 子订单号
        /// </summary>
        public string OrderItemNumber { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual long OrderId { get; set; }

        /// <summary>
        /// 订单
        /// </summary>
        public virtual Order Order { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public virtual int Quantity { get; set; }

        /// <summary>
        /// 单价
        /// </summary>
        public virtual decimal UnitPrice { get; set; }

        /// <summary>
        /// 价格（小计）
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 折扣
        /// </summary>
        public virtual decimal DiscountAmount { get; set; }

        /// <summary>
        /// 货物成本
        /// </summary>
        public virtual decimal OriginalProductCost { get; set; }

        /// <summary>
        /// 重量
        /// </summary>
        public virtual decimal Weight { get; set; }

        /// <summary>
        /// 体积
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 商品属性描述
        /// </summary>
        public virtual string AttributeDescription { get; set; }

        /// <summary>
        /// 商品属性json
        /// </summary>
        public virtual string AttributesJson { get; set; }
    }
}
