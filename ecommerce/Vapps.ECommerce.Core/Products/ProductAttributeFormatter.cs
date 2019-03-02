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
        /// Formats attributesXml
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesXml">Attributes</param>
        /// <returns>Attributes</returns>
        public string FormatAttributes(Product product, string attributesXml)
        {
            return FormatAttributes(product, attributesXml);
        }

        /// <summary>
        /// Formats attributesXml
        /// </summary>
        /// <param name="product">Product</param>
        /// <param name="attributesJson">Attributes</param>
        /// <param name="serapator">Serapator</param>
        /// <param name="htmlEncode">A value indicating whether to encode (HTML) values</param>
        /// <returns>Attributes</returns>
        public async Task<string> FormatAttributes(Product product, string attributesJson,
            string serapator = "<br />", bool htmlEncode = true)
        {
            var result = new StringBuilder();

            foreach (var attribute in await _productAttributeParser.ParseProductAttributeMappingsAsync(product.Id, attributesJson))
            {
                await _productAttributeManager.ProductAttributeMappingRepository.EnsurePropertyLoadedAsync(attribute, a => a.ProductAttribute);

                //attributes without values
                if (!attribute.ShouldHaveValues())
                {
                    foreach (var value in await _productAttributeParser.ParseProductAttributeValuesAsync(product.Id, attributesJson, attribute.Id))
                    {
                        var formattedAttribute = string.Empty;

                        //other attributes (textbox, datepicker)
                        formattedAttribute = string.Format("{0}: {1}", attribute.ProductAttribute.Name, value);

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
                    foreach (var attributeValue in await _productAttributeParser.ParseProductAttributeValuesAsync(product.Id, attributesJson, attribute.Id))
                    {
                        var formattedAttribute = string.Format("{0}: {1}",
                            attribute.ProductAttribute.Name,
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
