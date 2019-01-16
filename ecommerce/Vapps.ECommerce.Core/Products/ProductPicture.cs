using Abp.Domain.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Products
{
    /// <summary>
    /// 预约图片
    /// </summary>
    [Table("ProductPictures")]
    public class ProductPicture : Entity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public virtual long PictureId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public virtual int DisplayOrder { get; set; }
    }
}
