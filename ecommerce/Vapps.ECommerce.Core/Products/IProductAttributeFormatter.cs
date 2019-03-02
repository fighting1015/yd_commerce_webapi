using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// Product attribute formatter interface
    /// </summary>
    public partial interface IProductAttributeFormatter
    {
        /// <summary>
        /// Formats attributes
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesJson">Attributes</param>
        /// <returns>Attributes</returns>
        string FormatAttributes(Product product, string attributesJson);

        /// <summary>
        /// Formats attributesXml
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesJson">Attributes</param>
        /// <param name="serapator">Serapator</param>
        /// <param name="htmlEncode">A value indicating whether to encode (HTML) values</param>
        /// <returns>Attributes</returns>
        Task<string> FormatAttributes(Product product, string attributesJson,
            string serapator = "<br />", bool htmlEncode = true);
    }
}
