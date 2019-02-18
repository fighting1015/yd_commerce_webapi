using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;

namespace Vapps.ECommerce.Shipping
{
    /// <summary>
    /// 子物流单
    /// </summary>
    public partial class ShipmentItem : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 父物流单Id
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
