using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Products.Dto
{
    public class CreateOrUpdateAttributeValueInput : NullableIdDto<long>
    {
        /// <summary>
        /// 属性Id
        /// </summary>
        public long AttributeId { get; set; }

        /// <summary>
        /// 属性名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 排序标志
        /// </summary>
        public int DisplayOrder { get; set; }
    }

}
