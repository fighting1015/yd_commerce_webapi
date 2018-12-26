using Vapps.Editions;
using Vapps.Payments;

namespace Vapps.Payments.Dto
{
    public class CreatePaymentDto
    {
        /// <summary>
        /// 版本Id
        /// </summary>
        public int EditionId { get; set; }

        /// <summary>
        /// 购买来源
        /// </summary>
        public EditionPaymentType EditionPaymentType { get; set; }

        /// <summary>
        /// 购买时长
        /// </summary>
        public PaymentPeriodType? PaymentPeriodType { get; set; }

        /// <summary>
        /// 支付方式
        /// </summary>
        public SubscriptionPaymentGatewayType SubscriptionPaymentGatewayType { get; set; }
    }
}
