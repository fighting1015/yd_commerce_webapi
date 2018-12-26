namespace Vapps.Payments
{
    public class CreatePaymentRequest
    {
        public long? UserId { get; set; }

        public string PaymentId { get; set; }

        public string Description { get; set; }

        public decimal Amount { get; set; }
    }
}
