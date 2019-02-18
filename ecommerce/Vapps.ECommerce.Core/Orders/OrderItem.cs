using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Orders
{
    /// <summary>
    /// Represents an order item
    /// </summary>
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
        public virtual int OrderId { get; set; }

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
        /// 商品属性描述
        /// </summary>
        public virtual string AttributeDescription { get; set; }

        /// <summary>
        /// 商品属性json
        /// </summary>
        public virtual string AttributesJson { get; set; }
    }
}
