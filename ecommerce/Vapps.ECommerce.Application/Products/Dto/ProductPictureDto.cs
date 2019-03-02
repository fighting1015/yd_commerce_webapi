using Abp.Application.Services.Dto;

namespace Vapps.ECommerce.Products.Dto
{
    /// <summary>
    /// 产品图片
    /// </summary>
    public class ProductPictureDto 
    {
        /// <summary>
        /// 图片Id
        /// </summary>
        public virtual long Id { get; set; }

        /// <summary>
        /// 图片Url
        /// </summary>
        public virtual string Url { get; set; }
    }
}
