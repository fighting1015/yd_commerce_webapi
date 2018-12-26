using Abp.Application.Services.Dto;

namespace Vapps.Payments.Dto
{
    public class SubscriptionPaymentListDto : AuditedEntityDto
    {
        /// <summary>
        /// 支付方式
        /// </summary>
        public string Gateway { get; set; }

        /// <summary>
        /// 支付金额
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 版本Id
        /// </summary>
        public int EditionId { get; set; }

        /// <summary>
        /// 支付天数
        /// </summary>
        public int DayCount { get; set; }

        /// <summary>
        /// 支付周期类型
        /// </summary>
        public string PaymentPeriodType { get; set; }

        /// <summary>
        /// 支付订单号
        /// </summary>
        public string PaymentId { get; set; }

        /// <summary>
        /// 支付人Id(如果有)
        /// </summary>
        public string Payer { get; set; }

        /// <summary>
        /// 支付状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 版本显示名称
        /// </summary>
        public string EditionDisplayName { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int TenantId { get; set; }

        /// <summary>
        /// 第三方交易号
        /// </summary>
        public string InvoiceNo { get; set; }
    }
}
