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
        public int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductId { get; set; }

        /// <summary>
        /// 属性和值序列化字符串
        /// </summary>
        public string AttributesJson { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets the SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 价格覆盖
        /// </summary>
        public decimal? OverriddenPrice { get; set; }

        /// <summary>
        /// 成本覆盖
        /// </summary>
        public decimal? OverriddenGoodCost { get; set; }

        /// <summary>
        /// 第三方sku(仓储发货备用)
        /// </summary>
        public string ThirdPartySku { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }
    }
}
