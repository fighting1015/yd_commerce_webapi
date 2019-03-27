using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    public interface IProductAttributeParser
    {
        #region Product attributes

        /// <summary>
        /// 获取商品
        /// </summary>
        /// <param name="attributesJson">Attributes in XML format</param>
        /// <returns>Selected product</returns>
        Task<Product> ParseProductAsync(string attributesJson);

        /// <summary>
        /// 获取属性关联
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="attributesJson">Attributes in json format</param>
        /// <returns>Selected product attribute mappings</returns>
        Task<IList<ProductAttributeMapping>> ParseProductAttributeMappingsAsync(long productId,
            List<JsonProductAttribute> attributesJson);

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="attributesJson">格式化Json属性</param>
        /// <param name="productAttributeId"></param>
        /// <param name="productAttributeMappingId">属性关联Id,0为加载所有属性值</param>
        /// <returns></returns>
        Task<IList<ProductAttributeValue>> ParseProductAttributeValuesAsync(long productId,
            List<JsonProductAttribute> attributesJson,
            long productAttributeId = 0,
            long productAttributeMappingId = 0);


        #endregion

    }
}
