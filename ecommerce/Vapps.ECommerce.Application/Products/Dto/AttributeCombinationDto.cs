
using Abp.Application.Services.Dto;
using System.Collections.Generic;

namespace Vapps.ECommerce.Products.Dto
{
    /// <summary>
    /// 属性组合
    /// </summary>
    public partial class AttributeCombinationDto : EntityDto<long>
    {
        public AttributeCombinationDto()
        {
            this.Attributes = new List<ProductAttributeMappingDto>();
        }

        /// <summary>
        /// 属性值
        /// </summary>
        public List<ProductAttributeMappingDto> Attributes { get; set; }

        /// <summary>
        /// 库存
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// SKU
        /// </summary>
        public string Sku { get; set; }

        /// <summary>
        /// 第三方sku(仓储发货备用)
        /// </summary>
        public string ThirdPartySku { get; set; }

        /// <summary>
        /// 价格覆盖
        /// </summary>
        public decimal? OverriddenPrice { get; set; }

        /// <summary>
        /// 成本覆盖
        /// </summary>
        public decimal? OverriddenGoodCost { get; set; }
    }
}
