using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using System.Collections.Generic;
using System.Linq;

namespace Vapps.ECommerce.Products.Dto
{
    public static class DtoExtension
    {
        /// <summary>
        /// 初始化属性组合
        /// </summary>
        /// <param name="attributeMappings"></param>
        /// <param name="product"></param>
        /// <param name="productAttributeManager"></param>
        /// <param name="createOrUpdateProduct"></param>
        /// <returns></returns>
        [UnitOfWork]
        public static List<JsonProductAttribute> GetAttributesJson(this List<ProductAttributeDto> attributeMappings,
            Product product, IProductAttributeManager productAttributeManager, bool createOrUpdateProduct = false)
        {
            var jsonAttributes = new List<JsonProductAttribute>();

            foreach (var attributeDto in attributeMappings)
            {
                var jsonAttributeItem = new JsonProductAttribute()
                {
                    AttributeId = attributeDto.Id
                };

                foreach (var value in attributeDto.Values)
                {
                    long attributeValueId = 0;

                    if (createOrUpdateProduct)
                    {
                        var attributeValue = FindAttributValue(product, value, productAttributeManager, createOrUpdateProduct);
                        attributeValueId = attributeValue.Id;
                    }
                    else
                        attributeValueId = value.Id;

                    jsonAttributeItem.AttributeValues.Add(new JsonProductAttributeValue()
                    {
                        AttributeValueId = attributeValueId,
                    });
                }

                jsonAttributes.Add(jsonAttributeItem);
            }

            return jsonAttributes;
        }

        private static ProductAttributeValue FindAttributValue(Product product, ProductAttributeValueDto value,
            IProductAttributeManager productAttributeManager, bool createOrUpdateProduct = false)
        {
            ProductAttributeValue attributeValue = null;

            foreach (var attribute in product.Attributes)
            {
                productAttributeManager.ProductAttributeMappingRepository.EnsureCollectionLoaded(attribute, t => t.Values);

                if (createOrUpdateProduct)
                    attributeValue = attribute.Values.FirstOrDefault(v => v.PredefinedProductAttributeValueId == value.Id);
                else
                    attributeValue = attribute.Values.FirstOrDefault(v => v.Id == value.Id);

                if (attributeValue != null)
                    return attributeValue;

            }
            return attributeValue;
        }
    }
}
