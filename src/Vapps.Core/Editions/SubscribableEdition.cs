using Abp.Application.Editions;
using System;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Payments;

namespace Vapps.Editions
{
    /// <summary>
    /// 扩展 <see cref="Edition"/> 添加订阅功能
    /// </summary>
    public class SubscribableEdition : Edition
    {
        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 过期后指定版本
        /// The edition that will assigned after expire date
        /// </summary>
        public int? ExpiringEditionId { get; set; }

        /// <summary>
        /// 月价格
        /// </summary>
        public decimal? MonthlyPrice { get; set; }

        /// <summary>
        /// 季度价格
        /// </summary>
        public decimal? SeasonPrice { get; set; }

        /// <summary>
        /// 年价格
        /// </summary>
        public decimal? AnnualPrice { get; set; }

        /// <summary>
        /// 试用时间(天)
        /// </summary>
        public int? TrialDayCount { get; set; }

        /// <summary>
        /// 过期提醒时间(eg:过期前x天发送提醒)
        /// </summary>
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
