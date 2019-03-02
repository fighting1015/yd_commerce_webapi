using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Shippings.Dto.Tracking;

namespace Vapps.ECommerce.Shippings
{
    public interface IShippingTrackerAppService
    {
        /// <summary>
        /// 获取物流跟踪详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<TrackingDto> GetShipmentTracking(GetShipmentTrackingInput input);
    }
}
