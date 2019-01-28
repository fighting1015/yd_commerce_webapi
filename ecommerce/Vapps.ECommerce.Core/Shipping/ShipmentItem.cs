using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Vapps.ECommerce.Shipping
{
    /// <summary>
    /// 发货子项
    /// </summary>
    public partial class ShipmentItem : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 发货信息
        /// </summary>
        public int ShipmentId { get; set; }

        /// <summary>
        /// 订单item id
        /// </summary>
        public int OrderItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
