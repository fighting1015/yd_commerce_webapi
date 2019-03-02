using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products.Dto
{
    public class GetProductAttributeMappingOutput
    {
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
