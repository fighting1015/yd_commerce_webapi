using Abp.Application.Services.Dto;
using Abp.AutoMapper;

namespace Vapps.ECommerce.Products.Dto
{
    //[AutoMap(typeof(Product))]
    public class ProductListDto : EntityDto<long>
    {
        /// <summary>
        /// 商品名
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 短描述/推荐语
        /// </summary>
        public string ShortDescription { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 库存数量
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// 售价
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// 货物成本/进货价
        /// </summary>
        public decimal ProductCost { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
