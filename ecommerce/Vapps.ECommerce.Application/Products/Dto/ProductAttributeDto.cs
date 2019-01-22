using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace Vapps.ECommerce.Products.Dto
{
    /// <summary>
    /// 商品属性
    /// </summary>
    //[AutoMap(typeof(ProductAttribute))]
    public class ProductAttributeDto : EntityDto<long>
    {
        /// <summary>
        /// 商品属性名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预定义值/值记录
        /// </summary>
        public List<ProductAttributeValueDto> Values { get; set; }

        /// <summary>
        /// 排序Id
        /// </summary>
        public int DisplayOrder { get; set; }
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

        /// <summary>
        /// 排序Id
        /// </summary>
        public int DisplayOrder { get; set; }
    }

    /// <summary>
    /// 属性值
    /// </summary>
    [AutoMap(typeof(ProductAttributeValue))]
    public class ProductAttributeValueDto : EntityDto<long>
    {
        /// <summary>
        /// 属性名
        /// </summary>
        public string Attribute { get; set; }

        /// <summary>
        /// 属性Id
        /// </summary>
        public long AttributeId { get; set; }

        /// <summary>
        /// 属性值
        /// </summary>
        public string AttributeValue { get; set; }

        /// <summary>
        /// 属性值Id
        /// </summary>
        public long AttributeValueId { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 排序Id
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
