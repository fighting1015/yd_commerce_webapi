using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Stores
{
    /// <summary>
    /// 店铺关联
    /// </summary>
    [Table("StoreMappings")]
    public partial class StoreMapping : FullAuditedEntity
    {
        /// <summary>
        /// 实体Id
        /// </summary>
        public int EntityId { get; set; }

        /// <summary>
        /// Gets or sets the entity name
        /// </summary>
        public string EntityName { get; set; }

        /// <summary>
        /// Gets or sets the logistics identifier
        /// </summary>
        public int StoreId { get; set; }

        /// <summary>
        /// Gets or sets the logistics
        /// </summary>
        public virtual Store Store { get; set; }
    }
}
