using Abp.Domain.Repositories;
using Abp.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// Product attribute formatter
    /// </summary>
    public partial class ProductAttributeFormatter : VappsDomainServiceBase, IProductAttributeFormatter
    {
        private readonly IProductAttributeManager _productAttributeManager;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ILocalizationManager _localizationService;

        public ProductAttributeFormatter(IProductAttributeManager productAttributeManager,
            IProductAttributeParser productAttributeParser,
            ILocalizationManager localizationService)
        {
            this._productAttributeManager = productAttributeManager;
            this._productAttributeParser = productAttributeParser;
            this._localizationService = localizationService;
        }

        /// <summary>
        /// 格式化属性描述
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesJson">Attributes</param>
        /// <returns>Attributes</returns>
        public async Task<string> FormatAttributes(Product product,
            List<JsonProductAttribute> attributesJson)
        {
            return await FormatAttributes(product, attributesJson, "<br />", true);
        }

        /// <summary>
        /// 格式化属性描述
        /// </summary>
        /// <param name="product">商品</param>
        /// <param name="attributesJson">属性json对象</param>
        /// <param name="serapator">分隔符</param>
        /// <param name="htmlEncode">是否需要Html编码</param>
        /// <returns>格式化实行描述</returns>
        public async Task<string> FormatAttributes(Product product,
            List<JsonProductAttribute> attributesJson,
            string serapator = "<br />", bool htmlEncode = true)
        {
            var result = new StringBuilder();

            var attributeMappings = await _productAttributeParser.ParseProductAttributeMappingsAsync(product.Id, attributesJson);
            foreach (var attributeMapping in attributeMappings)
            {
                await _productAttributeManager.ProductAttributeMappingRepository.EnsurePropertyLoadedAsync(attributeMapping, a => a.ProductAttribute);

                //attributes without values
                if (!attributeMapping.ShouldHaveValues())
                {
                    foreach (var value in await _productAttributeParser.ParseProductAttributeValuesAsync(product.Id, attributesJson, attributeMapping.Id))
                    {
                        var formattedAttribute = string.Empty;

                        //other attributes (textbox, datepicker)
                        formattedAttribute = string.Format("{0}: {1}", attributeMapping.ProductAttribute.Name, value);

                        //encode (if required)
                        if (htmlEncode)
                            formattedAttribute = HttpUtility.HtmlEncode(formattedAttribute);

                        if (!string.IsNullOrEmpty(formattedAttribute))
                        {
                            if (result.Length > 0)
                                result.Append(serapator);
                            result.Append(formattedAttribute);
                        }
                    }
                }
                //product attribute values
                else
                {
                    foreach (var attributeValue in await _productAttributeParser.ParseProductAttributeValuesAsync(product.Id, attributesJson, attributeMapping.ProductAttributeId))
                    {
                        var formattedAttribute = string.Format("{0}: {1}",
                            attributeMapping.ProductAttribute.Name,
                            attributeValue.Name);

                        //encode (if required)
                        if (htmlEncode)
                            formattedAttribute = HttpUtility.HtmlEncode(formattedAttribute);

                        if (!string.IsNullOrEmpty(formattedAttribute))
                        {
                            if (result.Length > 0)
                                result.Append(serapator);
                            result.Append(formattedAttribute);
                        }
                    }
                }
            }

            return result.ToString();
        }
    }
}
