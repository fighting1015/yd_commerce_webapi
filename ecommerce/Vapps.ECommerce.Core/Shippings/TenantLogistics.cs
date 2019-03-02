using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Shippings
{
    /// <summary>
    /// 租户绑定快递公司
    /// </summary>
    [Table("TenantLogisticses")]
    public class TenantLogistics : CreationAuditedEntity<int>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 物流Id
        /// </summary>
        public virtual int LogisticsId { get; set; }
    }
}
