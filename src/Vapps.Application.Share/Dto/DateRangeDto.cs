using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.Dto
{
    public class DateRangeDto
    {
        /// <summary>
        /// 开始时间
        /// </summary>
        public DateTime? FormDate { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime? ToDate { get; set; }


    }

    public static class DateRangeDtoExtension
    {
        public static bool FormDateNotEmpty(this DateRangeDto value)
        {
            return value != null && value.FormDate != null;
        }

        public static bool ToDateNotEmpty(this DateRangeDto value)
        {
            return value != null && value.FormDate != null;
        }
    }
}
