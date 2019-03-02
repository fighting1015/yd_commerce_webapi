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
        /// <returns></returns>
        [UnitOfWork]
        public static List<JsonProductAttribute> GetAttributesJson(this List<ProductAttributeDto> attributeMappings,
            Product product)
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
                    var attributeValue = FindAttributValue(product, value);

                    jsonAttributeItem.AttributeValues.Add(new JsonProductAttributeValue()
                    {
                        AttributeValueId = attributeValue.Id,
                    });
                }

                jsonAttributes.Add(jsonAttributeItem);
            }

            return jsonAttributes;
        }

        private static ProductAttributeValue FindAttributValue(Product product, ProductAttributeValueDto value)
        {
            ProductAttributeValue attributeValue = null;

            foreach (var attribute in product.Attributes)
            {
                attributeValue = attribute.Values.FirstOrDefault(v => v.PredefinedProductAttributeValueId == value.Id);

                if (attributeValue != null)
                    return attributeValue;

            }
            return attributeValue;
        }
    }
}
