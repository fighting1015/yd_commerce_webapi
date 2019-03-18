using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Localization;
using Abp.Runtime.Caching;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.ECommerce.Orders;
using Vapps.ECommerce.Shippings.Dto.Tracking;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Shippings.Tracking;
using Abp.Authorization;
using Vapps.Authorization;

namespace Vapps.ECommerce.Shippings
{
    public class ShippingTrackerAppService : VappsAppServiceBase, IShippingTrackerAppService
    {
        private readonly IOrderManager _orderManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ICacheManager _cacheManager; // TODO: 待实现
        private readonly IShipmentManager _shipmentManager;
        private readonly IShipmentTracker _shipmentTracker;

        public ShippingTrackerAppService(IOrderManager orderManager,
            ILocalizationManager localizationManager,
            ICacheManager cacheManager,
            IShipmentManager shipmentManager,
            IShipmentTracker shipmentTracker)
        {
            this._orderManager = orderManager;
            this._localizationManager = localizationManager;
            this._cacheManager = cacheManager;
            this._shipmentManager = shipmentManager;
            this._shipmentTracker = shipmentTracker;
        }

        /// <summary>
        /// 获取物流跟踪详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.Shipment.Self)]
        public async Task<TrackingDto> GetShipmentTracking(GetShipmentTrackingInput input)
        {
            var resultDto = new TrackingDto()
            {
                Status = ShippingStatus.NotYetShipped.GetLocalizedEnum(_localizationManager)
            };

            Shipment shipment = null;

            if (input.OrderId.HasValue && input.OrderId > 0)
            {
                var order = await _orderManager.GetByIdAsync(input.OrderId.Value);
                if (order != null)
                {
                    await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, t => t.Shipments);
                    shipment = order.Shipments.FirstOrDefault();
                }
            }
            else
            {
                shipment = await _shipmentManager.GetByIdAsync(input.ShipmentId.Value);
            }

            if (await IsShippingSettingAvailable())
            {
                return resultDto;
            }

            var traces = await _shipmentTracker.GetShipmentTraces(shipment, input.Refresh);

            resultDto.Status = ((ShippingStatus)traces.State).GetLocalizedEnum(_localizationManager);
            resultDto.Traces = traces.Traces.Select(t =>
            {
                return new TrackingItemDto()
                {
                    Station = t.AcceptStation,
                    Time = t.AcceptTime,
                    Remark = t.Remark
                };
            }).ToList();

            return resultDto;
        }

        /// <summary>
        /// 判断快递接口是否可用
        /// </summary>
        /// <returns></returns>
        private async Task<bool> IsShippingSettingAvailable()
        {
            if ((await SettingManager.GetSettingValueAsync(AppSettings.Shipping.ApiId)).IsNullOrWhiteSpace())
                return false;

            if ((await SettingManager.GetSettingValueAsync(AppSettings.Shipping.ApiSecret)).IsNullOrWhiteSpace())
                return false;

            if ((await SettingManager.GetSettingValueAsync(AppSettings.Shipping.ApiUrl)).IsNullOrWhiteSpace())
                return false;

            return true;
        }
    }
}
