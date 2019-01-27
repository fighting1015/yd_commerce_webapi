using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.ECommerce.Products.Dto
{
    public static class DtoExtension
    {
        /// <summary>
        /// 初始化属性组合
        /// </summary>
        /// <param name="attributeMappings"></param>
        /// <returns></returns>
        public static List<JsonProductAttribute> GetAttributesJson(this List<ProductAttributeMappingDto> attributeMappings)
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
                    jsonAttributeItem.AttributeValues.Add(new JsonProductAttributeValue()
                    {
                        AttributeValueId = attributeDto.Id,
                        DisplayOrder = attributeDto.DisplayOrder,
                    });
                }

                jsonAttributes.Add(jsonAttributeItem);
            }

            return jsonAttributes;
        }
    }
}
