using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Catalog
{
    /// <summary>
    /// Represents a category
    /// </summary>
    [Table("Categories")]
    public partial class Category : FullAuditedEntity<long>, IMustHaveTenant, IPassivable
    {
        public const int MaxNameLength = 12;

        /// <summary>
        /// ×â»§Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the description
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Gets or sets the parent category identifier
        /// </summary>
        public virtual int ParentCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public virtual int PictureId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the entity is actived
        /// </summary>
        public virtual bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}