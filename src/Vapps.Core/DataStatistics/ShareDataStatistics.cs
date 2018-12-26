using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vapps.DataStatistics
{
    public class ShareDataStatistics
    {
        public ShareDataStatistics()
        {
            this.Times = new List<TimeShareData>();
        }

        /// <summary>
        /// 分享来源
        /// </summary>
        public string Target { get; set; }

        /// <summary>
        /// 分享数量
        /// </summary>
        public int Num { get; set; }

        public List<TimeShareData> Times { get; set; }
    }

    public class TimeShareData
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string HourOfDay { get; set; }

        /// <summary>
        /// 分享数量
        /// </summary>
        public int Num { get; set; }
    }
}
