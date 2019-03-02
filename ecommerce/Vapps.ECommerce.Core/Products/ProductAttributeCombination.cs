using Abp.Domain.Entities;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// 属性组合
    /// </summary>
    public partial class ProductAttributeCombination : Entity<long>, IMustHaveTenant, ISoftDelete
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
        /// Gets or sets the product 
        /// </summary>
        public virtual Product Product { get; set; }

        /// <summary>
        /// 属性和值序列化字符串
        /// </summary>
        public virtual string AttributesJson { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public virtual int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the SKU
        /// </summary>
        public virtual string Sku { get; set; }

        /// <summary>
        /// 价格覆盖
        /// </summary>
        public virtual decimal? OverriddenPrice { get; set; }

        /// <summary>
        /// 成本覆盖
        /// </summary>
        public virtual decimal? OverriddenGoodCost { get; set; }

        /// <summary>
        /// 第三方sku(仓储发货备用)
        /// </summary>
        public virtual string ThirdPartySku { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}
