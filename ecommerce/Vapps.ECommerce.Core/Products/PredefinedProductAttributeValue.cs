using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// 预定义属性值表
    /// </summary>
    [Table("PredefinedProductAttributeValues")]
    public class PredefinedProductAttributeValue : CreationAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 属性值主键
        /// </summary>
        public long ProductAttributeId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }
}
