using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// Represents a product category mapping
    /// </summary>
    [Table("ProductCategories")]
    public partial class ProductCategory : Entity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public virtual long CategoryId { get; set; }
    }
}
