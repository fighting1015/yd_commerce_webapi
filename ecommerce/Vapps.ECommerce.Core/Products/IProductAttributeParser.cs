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
        /// <param name="attributesJson">Attributes in json format</param>
        /// <returns>Selected product attribute mappings</returns>
        Task<IList<ProductAttributeMapping>> ParseProductAttributeMappingsAsync(long productId, string attributesJson);

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="attributesJson">格式化Json属性</param>
        /// <param name="productAttributeMappingId">属性关联Id,0为加载所有属性值</param>
        /// <returns>商品属性值</returns>
        Task<IList<ProductAttributeValue>> ParseProductAttributeValuesAsync(long productId, string attributesJson, long productAttributeId = 0, long productAttributeMappingId = 0);

        ///// <summary>
        ///// 添加商品属性
        ///// </summary>
        ///// <param name="attributesJson">Attributes in XML format</param>
        ///// <param name="productAttributeMapping">Product attribute mapping</param>
        ///// <param name="value">Value</param>
        ///// <returns>Updated result (XML format)</returns>
        //Task<string> AddProductAttribute(string attributesJson, ProductAttributeMapping productAttributeMapping, string value);

        ///// <summary>
        ///// Remove an attribute
        ///// </summary>
        ///// <param name="attributesJson">Attributes in XML format</param>
        ///// <param name="productAttributeMapping">Product attribute mapping</param>
        ///// <returns>Updated result (XML format)</returns>
        //string RemoveProductAttribute(string attributesJson, ProductAttributeMapping productAttributeMapping);

        ///// <summary>
        ///// Are attributes equal
        ///// </summary>
        ///// <param name="attributesJson1">The attributes of the first product</param>
        ///// <param name="attributesJson2">The attributes of the second product</param>
        ///// <param name="ignoreNonCombinableAttributes">A value indicating whether we should ignore non-combinable attributes</param>
        ///// <param name="ignoreQuantity">A value indicating whether we should ignore the quantity of attribute value entered by the customer</param>
        ///// <returns>Result</returns>
        //bool AreProductAttributesEqual(string attributesJson1, string attributesJson2, bool ignoreNonCombinableAttributes, bool ignoreQuantity = true);

        ///// <summary>
        ///// Check whether condition of some attribute is met (if specified). Return "null" if not condition is specified
        ///// </summary>
        ///// <param name="pam">Product attribute</param>
        ///// <param name="selectedAttributesXml">Selected attributes (XML format)</param>
        ///// <returns>Result</returns>
        //bool? IsConditionMet(ProductAttributeMapping pam, string selectedAttributesXml);

        ///// <summary>
        ///// Finds a product attribute combination by attributes stored in XML 
        ///// </summary>
        ///// <param name="product">Product</param>
        ///// <param name="attributesJson">Attributes in XML format</param>
        ///// <param name="ignoreNonCombinableAttributes">A value indicating whether we should ignore non-combinable attributes</param>
        ///// <returns>Found product attribute combination</returns>
        //ProductAttributeCombination FindProductAttributeCombination(Product product,
        //    string attributesJson, bool ignoreNonCombinableAttributes = true);

        ///// <summary>
        ///// Generate all combinations
        ///// </summary>
        ///// <param name="product">Product</param>
        ///// <param name="ignoreNonCombinableAttributes">A value indicating whether we should ignore non-combinable attributes</param>
        ///// <returns>Attribute combinations in XML format</returns>
        //IList<string> GenerateAllCombinations(Product product, bool ignoreNonCombinableAttributes = false);

        ///// <summary>
        ///// 生成AttributesXML 
        ///// </summary>
        ///// <param name="baseAttributeId"></param>
        ///// <param name="baseAttributeValueId"></param>
        ///// <param name="compareAttributeId"></param>
        ///// <param name="compareAttributeValueId"></param>
        ///// <returns></returns>
        //string CreateProductAttributeCombinationXML(int baseAttributeId, int baseAttributeValueId, int compareAttributeId, int compareAttributeValueId);

        #endregion

    }
}
