using System.Collections.Generic;

namespace Vapps.DataStatistics
{
    public class AccessDataStatistics
    {
        public AccessDataStatistics()
        {
            this.Channels = new List<ChannelAccessData>();
        }

        /// <summary>
        /// 渠道
        /// </summary>
        public List<ChannelAccessData> Channels { get; set; }
    }

    public class ChannelAccessData
    {
        public ChannelAccessData()
        {
            this.Regions = new List<RegionAccessData>();
        }

        /// <summary>
        /// 渠道名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 区域
        /// </summary>
        public List<RegionAccessData> Regions { get; set; }

        /// <summary>
        /// 是否微信渠道
        /// </summary>
        public bool IsWeChatChannel { get; set; }

        /// <summary>
        /// 访问数量
        /// </summary>
        public int Num { get; set; }
    }

    /// <summary>
    /// 区域名称
    /// </summary>
    public class RegionAccessData
    {
        public RegionAccessData()
        {
            this.Times = new List<TimeAccessData>();
        }

        /// <summary>
        /// 地区
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 访问时间
        /// </summary>
        public List<TimeAccessData> Times { get; set; }

        /// <summary>
        /// 访问数量
        /// </summary>
        public int Num { get; set; }
    }

    public class TimeAccessData
    {
        /// <summary>
        /// 时间
        /// </summary>
        public string HourOfDay { get; set; }

        /// <summary>
        /// 访问数量
        /// </summary>
        public int Num { get; set; }
    }

}
