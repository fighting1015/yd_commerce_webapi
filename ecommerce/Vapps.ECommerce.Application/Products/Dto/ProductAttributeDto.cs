using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace Vapps.ECommerce.Products.Dto
{
    /// <summary>
    /// 商品属性
    /// </summary>
    public class ProductAttributeDto : EntityDto<long>
    {
        /// <summary>
        /// 商品属性名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预定义值/值记录
        /// </summary>
        public List<PredefinedProductAttributeValue> Values { get; set; }
    }

    /// <summary>
    /// 预定义属性值
    /// </summary>
    public class PredefinedProductAttributeValue : EntityDto<long>
    {
        /// <summary>
        /// 属性id
        /// </summary>
        public int ProductAttributeId { get; set; }

        /// <summary>
        /// 名称
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 属性值
    /// </summary>
    [AutoMap(typeof(ProductAttributeValue))]
    public class ProductAttributeValueDto : EntityDto<long>
    {
        public int ProductAttributeId { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public int PictureId { get; set; }
    }
}
