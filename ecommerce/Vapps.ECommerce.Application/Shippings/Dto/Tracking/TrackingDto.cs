using System.Collections.Generic;

namespace Vapps.ECommerce.Shippings.Dto.Tracking
{


    /// <summary>
    /// 物流跟踪详情
    /// </summary>
    public class TrackingDto
    {
        public TrackingDto()
        {
            this.Traces = new List<TrackingItemDto>();
        }

        /// <summary>
        /// 快递名称
        /// </summary>
        public virtual string LogisticsName { get; set; }

        /// <summary>
        /// 快递单号
        /// </summary>
        public virtual string LogisticsNumber { get; set; }

        /// <summary>
        /// 状态
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// 物流信息
        /// </summary>
        public List<TrackingItemDto> Traces { get; set; }
    }

    public class TrackingItemDto
    {
        /// <summary>
        /// 物流信息条目
        /// </summary>
        public string Station { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public string Time { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }
}
