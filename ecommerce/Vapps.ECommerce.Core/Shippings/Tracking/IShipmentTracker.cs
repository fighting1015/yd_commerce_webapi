using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders;

namespace Vapps.ECommerce.Shippings.Tracking
{
    public interface IShipmentTracker
    {
        /// <summary>
        /// 物流信息订阅
        /// </summary>
        /// <returns></returns>
        Task<bool> OrderTracesSubscribeAsync(Shipment shipment);

        /// <summary>
        /// 获取物流信息
        /// </summary>
        /// <returns></returns>
        Task<TraceResult> GetShipmentTracesAsync(Shipment shipment, bool refresh);

        /// <summary>
        /// 更新物流信息
        /// </summary>
        /// <returns></returns>
        Task UpdateShipmentTracesAsync(List<TraceResult> traces);

        /// <summary>
        /// 更新物流信息
        /// </summary>
        /// <returns></returns>
        Task UpdateShipmentTracesAsync(TraceResult trace);

        /// <summary>
        /// 更新物流信息
        /// </summary>
        /// <param name="shipment"></param>
        /// <param name="trace"></param>
        /// <returns></returns>
        Task UpdateShipmentTracesAsync(Shipment shipment, TraceResult trace);
    }
}
