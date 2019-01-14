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
        public int TenantId { get; set; }

        /// <summary>
        /// 商品id
        /// </summary>
        public long ProductId { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public long PictureId { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
