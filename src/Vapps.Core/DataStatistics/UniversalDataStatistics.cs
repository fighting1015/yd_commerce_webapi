using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.DataStatistics
{
    [Table("UniversalDataStatistics")]
    public class UniversalDataStatistics : Entity<long>, IMustHaveTenant, IHasCreationTime
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 数据统计时间
        /// </summary>
        public virtual DateTime Date { get; set; }

        /// <summary>
        /// 数据统计结果
        /// </summary>
        public string DataStatistics { get; set; }

        /// <summary>
        /// 数据类型
        /// </summary>
        public UniversalDataType DataType { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public virtual DateTime CreationTime { get; set; }
    }
}
