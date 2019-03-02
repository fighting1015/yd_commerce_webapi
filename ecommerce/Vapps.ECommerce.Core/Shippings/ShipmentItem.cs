using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Shippings
{
    /// <summary>
    /// 子物流单
    /// </summary>
    [Table("ShipmentItems")]
    public partial class ShipmentItem : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 父物流单Id
        /// </summary>
        public long ShipmentId { get; set; }

        /// <summary>
        /// 订单item id
        /// </summary>
        public long OrderItemId { get; set; }

        /// <summary>
        /// 数量
        /// </summary>
        public int Quantity { get; set; }
    }
}
