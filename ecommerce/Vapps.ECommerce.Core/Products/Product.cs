using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.Collections.Generic;
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
        public virtual int TenantId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 短描述/推荐语
        /// </summary>
        public virtual string ShortDescription { get; set; }

        /// <summary>
        /// 长描述(Html)
        /// </summary>
        public virtual string FullDescription { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public virtual string Sku { get; set; }

        /// <summary>
        /// 第三方sku(仓储发货备用)
        /// </summary>
        public virtual string ThirdPartySku { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public virtual int StockQuantity { get; set; }

        /// <summary>
        /// 低库存通知（暂不实现）
        /// </summary>
        public virtual int NotifyAdminForQuantityBelow { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public virtual decimal Price { get; set; }

        /// <summary>
        /// 货物成本/进货价
        /// </summary>
        public virtual decimal ProductCost { get; set; }

        /// <summary>
        /// 重量（发货毛重）
        /// </summary>
        public virtual decimal Weight { get; set; }

        /// <summary>
        /// 长
        /// </summary>
        public virtual decimal Length { get; set; }

        /// <summary>
        /// 宽
        /// </summary>
        public virtual decimal Width { get; set; }

        /// <summary>
        /// 高
        /// </summary>
        public virtual decimal Height { get; set; }

        /// <summary>
        /// 分类
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductCategory> Categorys { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductPicture> Pictures { get; set; }

        /// <summary>
        /// 属性
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductAttributeMapping> Attributes { get; set; }

        /// <summary>
        /// 属性组合
        /// </summary>
        [ForeignKey("ProductId")]
        public virtual ICollection<ProductAttributeCombination> AttributeCombinations { get; set; }
    }
}
