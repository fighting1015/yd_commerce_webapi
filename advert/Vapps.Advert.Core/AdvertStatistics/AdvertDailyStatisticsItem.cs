using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.Advert.AdvertStatistics
{
    [Table("AdvertDailyStatisticItems")]
    public class AdvertDailyStatisticItem : CreationAuditedEntity<long>, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 广告统计外键Id
        /// </summary>
        public virtual long AdvertDailyStatisticId { get; set; }

        /// <summary>
        /// 统计时间
        /// </summary>
        public virtual int HourOfDay { get; set; }

        /// <summary>
        /// 展示数
        /// </summary>
        public virtual int DisplayNum { get; set; }

        /// <summary>
        /// 点击数
        /// </summary>
        public virtual int ClickNum { get; set; }

        /// <summary>
        /// 平均点击价格
        /// </summary>
        public virtual decimal ClickPrice { get; set; }

        /// <summary>
        /// 平均千次展现费用
        /// </summary>
        public virtual decimal ThDisplayCost { get; set; }

        /// <summary>
        /// 总花费
        /// </summary>
        public virtual decimal TotalCost { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }
    }
}
