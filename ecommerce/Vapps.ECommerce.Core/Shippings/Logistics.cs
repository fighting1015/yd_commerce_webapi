using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Shippings
{
    /// <summary>
    /// 快递公司(平台通用)
    /// </summary>
    [Table("Logisticses")]
    public class Logistics : FullAuditedEntity
    {
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
    }
}
