using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Orders;

namespace Vapps.Stores
{
    /// <summary>
    /// Represents a store
    /// </summary>
    [Table("Stores")]
    public class Store : FullAuditedEntity
    {
        /// <summary>
        /// 店铺名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 排序id
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int PictureId { get; set; }

        /// <summary>
        /// 第三方App id
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// 第三方App secret
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// 订单来源
        /// </summary>
        public OrderSource OrderSourceType { get; set; }
    }
}
