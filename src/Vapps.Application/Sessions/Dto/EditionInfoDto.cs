using Abp.Application.Services.Dto;
using System.ComponentModel.DataAnnotations.Schema;
using Vapps.Editions;

namespace Vapps.Sessions.Dto
{
    public class EditionInfoDto : EntityDto
    {
        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 试用时间
        /// </summary>
        public int? TrialDayCount { get; set; }

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
        /// 是否最高版本
        /// </summary>
        public bool IsHighestEdition { get; set; }

        /// <summary>
        /// 是否免费
        /// </summary>
        public bool IsFree { get; set; }

        public void SetEditionIsHighest(SubscribableEdition topEdition)
        {
            if (topEdition == null)
            {
                return;
            }

            IsHighestEdition = Id == topEdition.Id;
        }
    }
}