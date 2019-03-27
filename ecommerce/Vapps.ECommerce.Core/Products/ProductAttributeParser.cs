using Abp.Domain.Repositories;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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
        /// <param name="attributesJson">Attributes in json format</param>
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
        /// <param name="productId">商品id</param>
        /// <param name="attributesJson">属性json对象</param>
        /// <returns></returns>
        public virtual async Task<IList<ProductAttributeMapping>> ParseProductAttributeMappingsAsync(long productId,
            List<JsonProductAttribute> attributesJson)
        {
            var result = new List<ProductAttributeMapping>();
            if (attributesJson == null && attributesJson.Any())
                return result;

            foreach (var attribute in attributesJson)
            {
                var mapping = await _productAttributeManager.FindMappingAsync(productId, attribute.AttributeId);
                result.Add(mapping);
            }

            return result;
        }

        /// <summary>
        /// 获取属性值
        /// </summary>
        /// <param name="productId"></param>
        /// <param name="attributesJson">格式化Json属性</param>
        /// <param name="productAttributeId">属性Id,0为加载所有属性值</param>
        /// <param name="productAttributeMappingId">属性关联Id,0为加载所有属性值</param>
        /// <returns>商品属性值</returns>
        public virtual async Task<IList<ProductAttributeValue>> ParseProductAttributeValuesAsync(long productId,
            List<JsonProductAttribute> attributesJson,
            long productAttributeId = 0,
            long productAttributeMappingId = 0)
        {
            var values = new List<ProductAttributeValue>();
            if (attributesJson == null && attributesJson.Any())
                return values;

            // 获取商品属性
            var attributemappings = await ParseProductAttributeMappingsAsync(productId, attributesJson);

            // 根据属性Id过滤属性
            if (productAttributeId > 0)
                attributemappings = attributemappings.Where(attribute => attribute.ProductAttributeId == productAttributeId).ToList();

            // 根据map Id过滤属性
            if (productAttributeMappingId > 0)
                attributemappings = attributemappings.Where(attribute => attribute.Id == productAttributeMappingId).ToList();

            foreach (var mapping in attributemappings)
            {
                await _productAttributeManager.ProductAttributeMappingRepository.EnsureCollectionLoadedAsync(mapping, a => a.Values);

                if (!mapping.ShouldHaveValues())
                    continue;

                foreach (var attributeValue in await ParseValuesWithMappingIdAsync(productId, attributesJson, mapping.ProductAttributeId))
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
        /// <param name="productId"></param>
        /// <param name="attributesJson">Attributes in XML format</param>
        /// <returns>Selected product attribute mapping identifiers</returns>
        protected virtual async Task<IList<long>> ParseProductAttributeMappingIds(long productId,
            List<JsonProductAttribute> attributesJson)
        {
            var ids = new List<long>();
            if (attributesJson == null && attributesJson.Any())
                return ids;

            try
            {
                foreach (var attribute in attributesJson)
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
        /// 根据json属性，获取商品属性
        /// </summary>
        /// <param name="productId">商品Id</param>
        /// <param name="attributesJson">属性json对象</param>
        /// <param name="productAttributeId">属性id</param>
        /// <param name="productAttributeMappingId">属性关联id</param>
        /// <returns></returns>
        protected async Task<IList<ProductAttributeValue>> ParseValuesWithMappingIdAsync(long productId,
            List<JsonProductAttribute> attributesJson,
            long productAttributeId,
            long productAttributeMappingId = 0)
        {
            var values = new List<ProductAttributeValue>();
            if (attributesJson == null && attributesJson.Any())
                return values;

            try
            {
                foreach (var attribute in attributesJson)
                {
                    if (attribute.AttributeValues == null || !attribute.AttributeValues.Any())
                    {
                        continue;
                    }

                    if (productAttributeId != 0 && attribute.AttributeId != productAttributeId)
                    {
                        continue;
                    }

                    if (productAttributeMappingId != 0)
                    {
                        var mapping = await _productAttributeManager.FindMappingAsync(productId, attribute.AttributeId);
                        if (mapping.Id != productAttributeMappingId)
                            continue;
                    }

                    foreach (var jsonValue in attribute.AttributeValues)
                    {
                        var value = await _productAttributeManager.GetValueByIdAsync(jsonValue.AttributeValueId);

                        values.Add(value);
                    }
                }

                return values;
            }
            catch
            {

            }
            return values;
        }

        #endregion
    }
}
