using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Vapps.Editions.Dto
{
    /// <summary>
    /// 版本信息编辑 DTO
    /// </summary>
    [AutoMap(typeof(SubscribableEdition))]
    public class EditionEditDto
    {
        /// <summary>
        /// Id(可空)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 是否启用
        /// </summary>
        public bool IsEnable { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        public string DisplayName { get; set; }

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


        public int? WaitingDayAfterExpire { get; set; }

        /// <summary>
        /// 过期后
        /// </summary>
        public int? ExpiringEditionId { get; set; }
    }
}