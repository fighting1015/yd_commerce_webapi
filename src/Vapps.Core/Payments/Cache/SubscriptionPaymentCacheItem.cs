using Abp.AutoMapper;
using System;
using Vapps.Editions;

namespace Vapps.Payments.Cache
{
    [Serializable]
    public class SubscriptionPaymentCacheItem
    {
        public const string CacheName = "SubscriptionPaymentCache";

        public long Id { get; set; }

        public SubscriptionPaymentGatewayType Gateway { get; set; }

        public decimal Amount { get; set; }

        public SubscriptionPaymentStatus Status { get; set; }

        public int EditionId { get; set; }

        public int TenantId { get; set; }

        public int DayCount { get; set; }

        public PaymentPeriodType? PaymentPeriodType { get; set; }

        public EditionPaymentType EditionPaymentType { get; set; }

        public string PaymentId { get; set; }

        public string InvoiceNo { get; set; }
    }
}
