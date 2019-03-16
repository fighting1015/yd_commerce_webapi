using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace Vapps.ECommerce.Products.Dto
{
    //[AutoMap(typeof(Product))]
    public class CreateOrUpdateProductInput
    {

        public CreateOrUpdateProductInput() {
            Pictures = new List<ProductPictureDto>();
            Categories = new List<ProductCategoryDto>();
            Attributes = new List<ProductAttributeDto>();
            AttributeCombinations = new List<AttributeCombinationDto>();
        }

        /// <summary>
        /// Id，空或者为0时创建商品
        /// </summary>
        public long? Id { get; set; }

        /// <summary>
        /// 商品名
        /// </summary>
        public virtual string Name { get; set; }

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
        public int NotifyQuantityBelow { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 货物成本/进货价
        /// </summary>
        public decimal GoodCost { get; set; }

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

        /// <summary>
        /// 分类
        /// </summary>
        public virtual List<ProductCategoryDto> Categories { get; set; }

        /// <summary>
        /// 图片
        /// </summary>
        public virtual List<ProductPictureDto> Pictures { get; set; }

        /// <summary>
        /// 商品属性和值
        /// </summary>
        public virtual List<ProductAttributeDto> Attributes { get; set; }

        /// <summary>
        /// 商品属性组合
        /// </summary>
        public virtual List<AttributeCombinationDto> AttributeCombinations { get; set; }
    }
}
