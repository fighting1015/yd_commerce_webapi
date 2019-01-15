using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// Represents a product attribute
    /// </summary
    [Table("Products")]
    public class Product : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 短描述/推荐语
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// 长描述(Html)
        /// </summary>
        public string FullDescription { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 第三方sku(仓储发货备用)
        /// </summary>
        public string ThirdPartySku { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 低库存通知（暂不实现）
        /// </summary>
        public int NotifyAdminForQuantityBelow { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 货物成本/进货价
        /// </summary>
        public decimal ProductCost { get; set; }

        /// <summary>
        /// 重量（发货毛重）
        /// </summary>
        public decimal Weight { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public decimal Length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public decimal Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public decimal Height { get; set; }

    }
}
