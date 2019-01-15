using Abp.Domain.Entities;

namespace Vapps.ECommerce.Products
{
    public class ProductAttributeMapping : Entity<long>, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public int ProductAttributeId { get; set; }

        /// <summary>
        /// 排序id
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }
    }
}
