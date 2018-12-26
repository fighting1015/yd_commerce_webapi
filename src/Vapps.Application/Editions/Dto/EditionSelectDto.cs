using System.Collections.Generic;
using Abp.AutoMapper;
using Vapps.Payments;

namespace Vapps.Editions.Dto
{
    [AutoMap(typeof(SubscribableEdition))]
    public class EditionSelectDto
    {
        public int Id { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public virtual bool IsEnable { get; set; }

        /// <summary>
        /// 版本名称
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public virtual string DisplayName { get; set; }

        /// <summary>
        /// 过期版本
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
        /// 试用天数
        /// </summary>
        public int? TrialDayCount { get; set; }

        /// <summary>
        /// 过期后等待天数
        /// </summary>
        public int? WaitingDayAfterExpire { get; set; }

        /// <summary>
        /// 是否免费
        /// </summary>
        public bool IsFree { get; set; }

        public Dictionary<SubscriptionPaymentGatewayType, Dictionary<string, string>> AdditionalData { get; set; }

        public EditionSelectDto()
        {
            AdditionalData = new Dictionary<SubscriptionPaymentGatewayType, Dictionary<string, string>>();
        }
    }
}