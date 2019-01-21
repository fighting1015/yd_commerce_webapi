﻿using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Products.Dto
{
    /// <summary>
    /// 产品图片
    /// </summary>
    public class ProductPictureDto : NullableIdDto<long>
    {
        /// <summary>
        /// 图片Id
        /// </summary>
        public virtual long PictureId { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public virtual string PictureUrl { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}