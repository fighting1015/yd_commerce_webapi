using System.Collections.Generic;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// Product attribute formatter interface
    /// </summary>
    public partial interface IProductAttributeFormatter
    {
        /// <summary>
        /// 格式化属性描述
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesJson">Attributes</param>
        /// <returns>Attributes</returns>
        Task<string> FormatAttributesAsync(Product product, List<JsonProductAttribute> attributesJson);

        /// <summary>
        /// 格式化属性描述
        /// </summary>
        /// <param name="product">商品</param>
        /// <param name="attributesJson">属性json对象</param>
        /// <param name="serapator">分隔符</param>
        /// <param name="htmlEncode">是否需要Html编码</param>
        /// <returns>格式化实行描述</returns>
        Task<string> FormatAttributesAsync(Product product, List<JsonProductAttribute> attributesJson,
            string serapator = "<br />", bool htmlEncode = true);
    }
}
