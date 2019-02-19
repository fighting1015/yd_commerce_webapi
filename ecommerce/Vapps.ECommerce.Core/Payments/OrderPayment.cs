using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.ECommerce.Payments
{
    /// <summary>
    /// 订单支付表
    /// </summary>
    [Table("OrderPaymsents")]
    public class OrderPaymsent : FullAuditedEntity<long>, IMustHaveTenant
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public virtual int TenantId { get; set; }

        /// <summary>
        /// 订单Id
        /// </summary>
        public virtual long OrderId { get; set; }

        /// <summary>
        /// 外部交易订单号
        /// </summary>
        public virtual string ExternalOrderNumber { get; set; }

        /// <summary>
        /// 付款金额
        /// </summary>
        public virtual decimal Amount { get; set; }

        /// <summary>
        /// 付款方式
        /// </summary>
        public virtual PaymentMethod PaymentMethod { get; set; }

        /// <summary>
        /// 付款银行名称(如果有)
        /// </summary>
        public virtual string PaymentBankSystemName { get; set; }

        /// <summary>
        /// 付款者(来源第三方)
        /// </summary>
        public virtual string Payer { get; set; }
    }
}
