using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Products.Dto
{
    /// <summary>
    /// 预约图片
    /// </summary>
    public class ProductPictureDto : EntityDto<long>
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
