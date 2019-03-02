using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders;

namespace Vapps.ECommerce.Shippings.Tracking
{
    public interface IShipmentTracker
    {
        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <returns></returns>
        Task<TraceResult> GetShipmentTraces(Shipment shipment, bool refresh);

        /// <summary>
        /// 更新物流信息
        /// </summary>
        /// <returns></returns>
        Task UpdateShipmentTraces(List<TraceResult> traces);

        /// <summary>
        /// 更新物流信息
        /// </summary>
        /// <returns></returns>
        Task UpdateShipmentTracesAsync(TraceResult trace);
    }
}
