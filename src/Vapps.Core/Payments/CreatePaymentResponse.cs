using System.Collections.Generic;

namespace Vapps.Payments
{
    public class CreatePaymentResponse
    {
        /// <summary>
        /// 支付参数
        /// </summary>
        public Dictionary<string, string> AdditionalData { get; set; }

        /// <summary>
        /// 总价格
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// 订单号
        /// </summary>
        public string PaymentId { get; set; }

        public CreatePaymentResponse()
        {
            this.AdditionalData = new Dictionary<string, string>();
        }
    }
}