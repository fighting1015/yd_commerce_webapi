﻿using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.Collections.Generic;

namespace Vapps.ECommerce.Products.Dto
{
    /// <summary>
    /// 商品属性
    /// </summary>
    public class ProductAttributeListDto : EntityDto<long>
    {
        /// <summary>
        /// 商品属性名
        /// </summary>
        public string Name { get; set; }
    }

    /// <summary>
    /// 商品属性
    /// </summary>
    //[AutoMap(typeof(ProductAttribute))]
    public class ProductAttributeDto : EntityDto<long>
    {
        public ProductAttributeDto()
        {
            Values = new List<ProductAttributeValueDto>();
        }

        /// <summary>
        /// 商品属性名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 预定义值/值记录
        /// </summary>
        public List<ProductAttributeValueDto> Values { get; set; }
    }

    /// <summary>
    /// 预定义属性值
    /// </summary>
    public class PredefinedProductAttributeValueDto : EntityDto<long>
    {
        /// <summary>
        /// 值
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
    public class ProductAttributeValueDto : EntityDto<long>
    {
        /// <summary>
        /// 值名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 图片id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public string PictureUrl { get; set; }
    }
}
