using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Advert.AdvertAccounts;

namespace Vapps.Advert.AdvertStatistics
{
    [Table("AdvertDailyStatistics")]
    public class AdvertDailyStatistic : CreationAuditedEntity<long>, IMustHaveTenant, ISoftDelete
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 商品Id
        /// </summary>
        public virtual long ProductId { get; set; }

        /// <summary>
        /// 商品名称
        /// </summary>
        public virtual string ProductName { get; set; }

        /// <summary>
        /// 广告账户
        /// </summary>
        [ForeignKey("AdvertAccountId")]
        public virtual AdvertAccount AdvertAccount { get; set; }

        /// <summary>
        /// 广告账户Id
        /// </summary>
        public virtual long AdvertAccountId { get; set; }

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
        /// 统计日期
        /// </summary>
        public virtual DateTime StatisticOn { get; set; }

        /// <summary>
        /// 是否删除
        /// </summary>
        public virtual bool IsDeleted { get; set; }

        /// <summary>
        /// 条目
        /// </summary>
        [ForeignKey("AdvertStatisticsId")]
        public virtual ICollection<AdvertDailyStatisticItem> Items { get; set; }
    }
}
