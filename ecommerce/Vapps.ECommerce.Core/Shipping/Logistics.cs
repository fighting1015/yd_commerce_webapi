using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Shipping
{
    /// <summary>
    /// 快递公司
    /// </summary>
    [Table("Logisticses")]
    public class Logistics : FullAuditedEntity<long>, IMayHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// 租户Id为0时，为平台默认模板物流，有值时为租户添加物流
        /// </summary>
        public virtual int? TenantId { get; set; }

        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 快递 Key
        /// </summary>
        public virtual string Key { get; set; }

        /// <summary>
        /// 快递 缩写
        /// </summary>
        public virtual string Memo { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}
