using Abp.AutoMapper;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Payments;

namespace Vapps.Editions.Cache
{
    [AutoMapFrom(typeof(SubscribableEdition))]
    public class SubscribableEditionCacheItem
    {
        public static string CacheName => "SubscribableEditionCache";

        public static string DefaultEditionCacheName => "DefaultSubscribableEditionCache";

        public static string HighestEditionCacheName => "TopSubscribableEditionCache";

        public int Id { get; set; }

        public bool IsEnable { get; set; }

        public string Name { get; set; }
        public string DisplayName { get; set; }

        public int? ExpiringEditionId { get; set; }

        public decimal? MonthlyPrice { get; set; }

        public decimal? SeasonPrice { get; set; }

        public decimal? AnnualPrice { get; set; }

        public int? TrialDayCount { get; set; }

        public int? WaitingDayAfterExpire { get; set; }

        [NotMapped]
        public bool IsFree => !MonthlyPrice.HasValue && !AnnualPrice.HasValue;

        /// <summary>
        /// 能否试用
        /// </summary>
        /// <returns></returns>
        public bool HasTrial()
        {
            if (IsFree)
            {
                return false;
            }

            return TrialDayCount.HasValue && TrialDayCount.Value > 0;
        }

        /// <summary>
        /// 获取支付价格
        /// </summary>
        /// <param name="paymentPeriodType"></param>
        /// <returns></returns>
        public decimal GetPaymentAmount(PaymentPeriodType? paymentPeriodType)
        {
            if (MonthlyPrice == null || AnnualPrice == null)
            {
                throw new Exception("No price information found for " + DisplayName + " edition!");
            }

            switch (paymentPeriodType)
            {
                case PaymentPeriodType.Monthly:
                    return MonthlyPrice.Value;
                case PaymentPeriodType.Season:
                    return SeasonPrice.Value;
                case PaymentPeriodType.Annual:
                    return AnnualPrice.Value;
                default:
                    throw new Exception("Edition does not support payment type: " + paymentPeriodType);
            }
        }

        /// <summary>
        /// 相同价格
        /// </summary>
        /// <param name="edition"></param>
        /// <returns></returns>
        public bool HasSamePrice(SubscribableEdition edition)
        {
            return !IsFree &&
                   MonthlyPrice == edition.MonthlyPrice && AnnualPrice == edition.AnnualPrice;
        }
    }
}
