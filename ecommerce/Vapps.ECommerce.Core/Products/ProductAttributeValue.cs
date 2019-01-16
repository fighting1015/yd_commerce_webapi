using Abp.Domain.Entities;

namespace Vapps.ECommerce.Products
{
    public partial class ProductAttributeValue : Entity<long>, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 属性关联Id
        /// </summary>
        public int ProductAttributeMappingId { get; set; }

        /// <summary>
        /// 产品Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public bool IsDeleted { get; set; }
    }
}
