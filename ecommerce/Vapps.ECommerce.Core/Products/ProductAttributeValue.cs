using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Products
{
    [Table("ProductAttributeValues")]
    public partial class ProductAttributeValue : Entity<long>, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 属性关联Id
        /// </summary>
        public virtual long ProductAttributeMappingId { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        public virtual ProductAttributeMapping ProductAttributeMapping { get; set; }

        /// <summary>
        /// 预设值Id
        /// </summary>
        public virtual long PredefinedProductAttributeValueId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 商品
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public virtual long PictureId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }
    }
}
