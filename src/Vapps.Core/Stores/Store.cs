using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Orders;

namespace Vapps.Stores
{
    /// <summary>
    /// Represents a store
    /// </summary>
    [Table("Stores")]
    public class Store : FullAuditedEntity, IMustHaveTenant
    {
        public const int MaxNameFieldLength = 12;

        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 店铺名
        /// </summary>
        [Required]
        [StringLength(MaxNameFieldLength)]
        public virtual string Name { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public virtual string Url { get; set; }

        /// <summary>
        /// 排序id
        /// </summary>
        public virtual int DisplayOrder { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public virtual int PictureId { get; set; }

        /// <summary>
        /// 第三方App id
        /// </summary>
        public virtual string AppKey { get; set; }

        /// <summary>
        /// 第三方App secret
        /// </summary>
        public virtual string AppSecret { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public virtual OrderSource OrderSourceType { get; set; }
    }
}
