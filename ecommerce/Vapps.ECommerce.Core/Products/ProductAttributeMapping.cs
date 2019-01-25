using Abp.Domain.Entities;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Products
{
    [Table("ProductAttributeMappings")]
    public class ProductAttributeMapping : Entity<long>, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public virtual long ProductAttributeId { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public virtual ProductAttribute ProductAttribute { get; set; }

        /// <summary>
        /// 排序id
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public virtual ICollection<ProductAttributeValue> Values { get; set; }
    }
}
