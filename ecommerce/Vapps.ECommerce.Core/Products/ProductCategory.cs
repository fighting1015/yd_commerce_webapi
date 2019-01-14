using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// Represents a product category mapping
    /// </summary>
    [Table("ProductPictures")]
    public partial class ProductCategory : Entity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// Gets or sets the category identifier
        /// </summary>
        public long CategoryId { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
