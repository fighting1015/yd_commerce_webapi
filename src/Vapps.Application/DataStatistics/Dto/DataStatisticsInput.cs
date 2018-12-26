using System.ComponentModel.DataAnnotations;

namespace Vapps.DataStatistics.Dto
{
    public class DataStatisticsInput
    {
        /// <summary>
        /// 数据统计日期
        /// </summary>
        [Required]
        public string Date { get; set; }
    }
}
