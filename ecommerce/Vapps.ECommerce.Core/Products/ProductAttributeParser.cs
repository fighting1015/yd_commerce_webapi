using Abp.Domain.Repositories;
using Castle.Core.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products
{
    public class ProductAttributeParser : VappsDomainServiceBase, IProductAttributeParser
    {
        private readonly IProductManager _productManager;
        private readonly IProductAttributeManager _productAttributeManager;
        private readonly ILogger _logger;

        public ProductAttributeParser(IProductManager productManager,
            ILogger logger,
            IProductAttributeManager productAttributeManager)
        {
            this._productManager = productManager;
            this._logger = logger;
            this._productAttributeManager = productAttributeManager;
        }

        /// <summary>
        /// 根据Json 查找产品
        /// </summary>
        /// <param name="attributesXml">Attributes in json format</param>
        /// <returns>Selected product</returns>
        public virtual async Task<Product> ParseProductAsync(string attributesJson)
        {
            var combin = await _productAttributeManager.FindCombinationByAttributesJsonAsync(attributesJson);

            if (combin == null)
                return null;

            await _productAttributeManager.ProductAttributeCombinationRepository
                .EnsurePropertyLoadedAsync(combin, c => c.Product);

            return combin.Product;
        }

        /// <summary>
        /// 根据Json 查找商品属性
        /// </summary>
        /// <param name="attributesXml">Attributes in Json format</param>
        /// <returns>Selected product attribute mappings</returns>
        public virtual async Task<IList<ProductAttributeMapping>> ParseProductAttributeMappingsAsync(long productId, string attributesJson)
        {
            var result = new List<ProductAttributeMapping>();
            if (String.IsNullOrEmpty(attributesJson))
                return result;

            var attributes = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(attributesJson);
            foreach (var attribute in attributes)
            {
                var mapping = await _productAttributeManager.FindMappingAsync(productId, attribute.AttributeId);
                result.Add(mapping);
            }

            return result;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="attributesJson">格式化Json属性</param>
        /// <param name="productAttributeId">属性Id,0为加载所有属性值</param>
        /// <param name="productAttributeMappingId">属性关联Id,0为加载所有属性值</param>
        /// <returns>商品属性值</returns>
        public virtual async Task<IList<ProductAttributeValue>> ParseProductAttributeValuesAsync(long productId, string attributesJson, long productAttributeId = 0, long productAttributeMappingId = 0)
        {
            var values = new List<ProductAttributeValue>();
            if (string.IsNullOrEmpty(attributesJson))
                return values;

            var attributes = await ParseProductAttributeMappingsAsync(productId, attributesJson);

            //to load values only for the passed product attribute
            if (productAttributeMappingId > 0)
                attributes = attributes.Where(attribute => attribute.ProductAttributeId == productAttributeId).ToList();

            //to load values only for the passed product attribute mapping
            if (productAttributeMappingId > 0)
                attributes = attributes.Where(attribute => attribute.Id == productAttributeMappingId).ToList();

            foreach (var attribute in attributes)
            {
                if (!attribute.ShouldHaveValues())
                    continue;

                foreach (var attributeValue in await ParseValuesWithMappingIdAsync(productId, attributesJson, attribute.Id))
                {
                    values.Add(attributeValue);
                }
            }
            return values;
        }

        #region Utilites

        /// <summary>
        /// Gets selected product attribute mapping identifiers
        /// </summary>
        /// <param name="attributesJson">Attributes in XML format</param>
        /// <returns>Selected product attribute mapping identifiers</returns>
        protected virtual async Task<IList<long>> ParseProductAttributeMappingIds(long productId, string attributesJson)
        {
            var ids = new List<long>();
            if (String.IsNullOrEmpty(attributesJson))
                return ids;

            try
            {
                var attributes = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(attributesJson);
                foreach (var attribute in attributes)
                {
                    var mapping = await _productAttributeManager.FindMappingAsync(productId, attribute.AttributeId);
                    ids.Add(mapping.Id);
                }
            }
            catch (Exception exc)
            {
                _logger.Error(exc.ToString());
            }
            return ids;
        }

        /// <summary>
        /// Gets selected product attribute values with the quantity entered by the customer
        /// </summary>
        /// <param name="attributesXml">Attributes in Json format</param>
        /// <param name="productAttributeMappingId">Product attribute mapping identifier</param>
        /// <returns>Collections of pairs of product attribute values and their quantity</returns>
        protected async Task<IList<ProductAttributeValue>> ParseValuesWithMappingIdAsync(long productId, string attributesJson, long productAttributeMappingId)
        {
            var values = new List<ProductAttributeValue>();
            if (string.IsNullOrEmpty(attributesJson))
                return values;

            try
            {
                var attributes = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(attributesJson);
                foreach (var attribute in attributes)
                {
                    var mapping = await _productAttributeManager.FindMappingAsync(productId, attribute.AttributeId);
                    if (attribute.AttributeValues == null || attribute.AttributeValues.Any())
                    {
                        continue;
                    }

                    if (productAttributeMappingId != 0 && mapping.Id != productAttributeMappingId)
                    {
                        continue;
                    }

                    foreach (var jsonValue in attribute.AttributeValues)
                    {
                        var value = await _productAttributeManager.FindValueByPredefinedValueIdAsync(productId, jsonValue.AttributeValueId);

                        values.Add(value);
                    }
                }

                return values;
            }
            catch { }

            return values;
        }

        #endregion
    }
}
