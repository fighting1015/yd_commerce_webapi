using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Customer
{
    /// <summary>
    /// 租户客户
    /// </summary>
    [Table("Customers")]
    public class Customer : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public virtual int UserId { get; set; }

        /// <summary>
        /// 客户名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public virtual string PhoneNumber { get; set; }

        /// <summary>
        /// 消费总额
        /// </summary>
        public decimal TotalConsumption { get; set; }

        /// <summary>
        /// 购买频次
        /// </summary>
        public decimal TotalOrderNum { get; set; }
    }
}
