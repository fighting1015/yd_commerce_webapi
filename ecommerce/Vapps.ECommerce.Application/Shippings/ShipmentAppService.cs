using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Configuration;
using Vapps.Dto;
using Vapps.ECommerce.Orders;
using Vapps.ECommerce.Shippings.Dto.Shipments;
using Vapps.ECommerce.Shippings.Dto.Tracking;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Shippings.Tracking;
using Abp.Domain.Repositories;
using System.Collections.ObjectModel;

namespace Vapps.ECommerce.Shippings
{
    public class ShipmentAppService : VappsAppServiceBase, IShipmentAppService
    {
        private readonly IShipmentManager _shipmentManager;
        private readonly IOrderManager _orderManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ICacheManager _cacheManager; // TODO: 待实现
        private readonly IShipmentTracker _shipmentTracker;
        private readonly ILogisticsManager _logisticsManager;


        public ShipmentAppService(IShipmentManager shipmentManager,
            IOrderManager orderManager,
            ILocalizationManager localizationManager,
            ICacheManager cacheManager,
            IShipmentTracker shipmentTracker,
            ILogisticsManager logisticsManager)
        {
            this._shipmentManager = shipmentManager;
            this._orderManager = orderManager;
            this._cacheManager = cacheManager;
            this._localizationManager = localizationManager;
            this._shipmentTracker = shipmentTracker;
            this._logisticsManager = logisticsManager;
        }

        #region Shipment

        /// <summary>
        /// 获取所有发货记录
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<ShipmentListDto>> GetShipments(GetShipmentsInput input)
        {
            var query = _shipmentManager.Shipments
                .Include(p => p.Items)
                .WhereIf(input.Status != null, r => r.Status == input.Status)
                .WhereIf(input.TrackingNumber.IsNullOrWhiteSpace(), r => r.LogisticsNumber == input.TrackingNumber)
                .WhereIf(input.DeliveryFrom != null, r => r.CreationTime >= input.DeliveryFrom)
                .WhereIf(input.DeliveryTo != null, r => r.CreationTime <= input.DeliveryTo)
                .WhereIf(input.ReceivedFrom != null, r => r.ReceivedOn >= input.ReceivedFrom)
                .WhereIf(input.DeliveryTo != null, r => r.ReceivedOn <= input.DeliveryTo);

            var shipmentCount = await query.CountAsync();

            var shipmentes = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var shipmentListDtos = ObjectMapper.Map<List<ShipmentListDto>>(shipmentes);
            return new PagedResultDto<ShipmentListDto>(
                shipmentCount,
                shipmentListDtos);
        }

        /// <summary>
        /// 获取订单发货记录详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<List<ShipmentDto>> GetOrderShipments(long orderId)
        {
            List<ShipmentDto> shipmentDtoList = new List<ShipmentDto>();
            var shipments = _shipmentManager.FindByOrderId(orderId);

            if (shipments.IsNullOrEmpty())
                return shipmentDtoList;

            foreach (var shipment in shipments)
            {
                var shipmentDto = ObjectMapper.Map<ShipmentDto>(shipment);
                if (shipment.LogisticsId.HasValue)
                {
                    shipmentDto.LogisticsId = (await _logisticsManager.FindTenantLogisticsByLogisticsIdAsync(shipment.LogisticsId.Value)).Id;
                }

            }

            return shipmentDtoList;
        }

        /// <summary>
        /// 获取发货记录详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetShipmentForEditOutput> GetShipmentForEdit(NullableIdDto<int> input)
        {
            GetShipmentForEditOutput ShipmentDto;

            if (input.Id.HasValue)
            {
                var store = await _shipmentManager.GetByIdAsync(input.Id.Value);
                ShipmentDto = ObjectMapper.Map<GetShipmentForEditOutput>(store);
            }
            else
            {
                ShipmentDto = new GetShipmentForEditOutput();
            }

            return ShipmentDto;
        }

        /// <summary>
        /// 创建或更新发货记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntityDto<long>> CreateOrUpdateShipment(CreateOrUpdateShipmentInput input)
        {
            Shipment shipment = null;
            if (input.Id > 0)
            {
                shipment = await UpdateShipmentAsync(input);
            }
            else
            {
                shipment = await CreateShipmentAsync(input);
            }

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long> { Id = shipment.Id };
        }

        /// <summary>
        /// 删除发货记录
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteShipment(BatchInput<int> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _shipmentManager.DeleteAsync(id);
            }
        }

        #endregion

        #region Utilities

        /// <summary>
        /// 创建物流
        /// </summary>
        /// <returns></returns>
        protected virtual async Task<Shipment> CreateShipmentAsync(CreateOrUpdateShipmentInput input)
        {
            var shipment = ObjectMapper.Map<Shipment>(input);

            CreateOrUpdateOrderItem(input, shipment);

            await _shipmentManager.CreateAsync(shipment);

            return shipment;
        }

        /// <summary>
        /// 更新物流
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual async Task<Shipment> UpdateShipmentAsync(CreateOrUpdateShipmentInput input)
        {
            var shipment = await _shipmentManager.GetByIdAsync(input.Id);

            ObjectMapper.Map(input, shipment);

            await _shipmentManager.ShipmentRepository.EnsureCollectionLoadedAsync(shipment, s => s.Items);

            CreateOrUpdateOrderItem(input, shipment);

            await _shipmentManager.UpdateAsync(shipment);

            return shipment;
        }

        /// <summary>
        /// 创建更新物流条目
        /// </summary>
        /// <param name="input"></param>
        /// <param name="shipment"></param>
        private void CreateOrUpdateOrderItem(CreateOrUpdateShipmentInput input, Shipment shipment)
        {
            if (input.Id == 0)
            {
                shipment.Items = new Collection<ShipmentItem>();
            }
            else
            {
                var existItemIds = input.Items.Select(i => i.Id).ToList();
                var itemsId2Remove = shipment.Items.Where(i => !existItemIds.Contains(i.Id)).ToList();

                //删除不存在的属性
                foreach (var item in itemsId2Remove)
                {
                    item.IsDeleted = true;
                    shipment.Items.Remove(item);
                }
            }

            foreach (var itemDto in input.Items)
            {
                ShipmentItem item = null;
                if (itemDto.Id != 0)
                {
                    item = shipment.Items.FirstOrDefault(i => i.Id == item.Id);
                }

                if (item != null)
                {
                    ObjectMapper.Map(itemDto, item);
                }
                else
                {
                    item = ObjectMapper.Map<ShipmentItem>(itemDto);
                    shipment.Items.Add(item);
                }
            }
        }

        #endregion
    }
}
