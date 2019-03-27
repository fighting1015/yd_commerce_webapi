using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Caching;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Dto;
using Vapps.ECommerce.Orders;
using Vapps.ECommerce.Shippings.Dto.Shipments;
using Vapps.ECommerce.Shippings.Tracking;

namespace Vapps.ECommerce.Shippings
{
    [AbpAuthorize(BusinessCenterPermissions.SalesManage.Shipment.Self)]
    public class ShipmentAppService : VappsAppServiceBase, IShipmentAppService
    {
        private readonly IShipmentManager _shipmentManager;
        private readonly IOrderManager _orderManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly ICacheManager _cacheManager; // TODO: 待实现
        private readonly IShipmentTracker _shipmentTracker;
        private readonly ILogisticsManager _logisticsManager;
        private readonly IOrderProcessingManager _orderProcessingManager;


        public ShipmentAppService(IShipmentManager shipmentManager,
            IOrderManager orderManager,
            ILocalizationManager localizationManager,
            ICacheManager cacheManager,
            IShipmentTracker shipmentTracker,
            ILogisticsManager logisticsManager,
            IOrderProcessingManager orderProcessingManager)
        {
            this._shipmentManager = shipmentManager;
            this._orderManager = orderManager;
            this._cacheManager = cacheManager;
            this._localizationManager = localizationManager;
            this._shipmentTracker = shipmentTracker;
            this._logisticsManager = logisticsManager;
            this._orderProcessingManager = orderProcessingManager;
        }

        #region Shipment

        /// <summary>
        /// 获取所有发货记录
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.SalesManage.Shipment.Self)]
        public async Task<PagedResultDto<ShipmentListDto>> GetShipments(GetShipmentsInput input)
        {
            var query = _shipmentManager.Shipments
                .Include(p => p.Items)
                .WhereIf(input.Status != null, r => r.Status == input.Status)
                .WhereIf(!input.TrackingNumber.IsNullOrWhiteSpace(), r => r.LogisticsNumber == input.TrackingNumber)
                .WhereIf(input.DeliveriedOn.FormDateNotEmpty(), r => r.CreationTime >= input.DeliveriedOn.FormDate)
                .WhereIf(input.DeliveriedOn.ToDateNotEmpty(), r => r.CreationTime >= input.DeliveriedOn.ToDate)
                .WhereIf(input.ReceivedOn.FormDateNotEmpty(), r => r.CreationTime >= input.ReceivedOn.FormDate)
                .WhereIf(input.ReceivedOn.ToDateNotEmpty(), r => r.CreationTime >= input.ReceivedOn.ToDate);

            var shipmentCount = await query.CountAsync();

            var shipmentes = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var shipmentListDtos = await PrepareShipmentListDto(shipmentes);
            return new PagedResultDto<ShipmentListDto>(
                shipmentCount,
                shipmentListDtos);
        }

        /// <summary>
        /// 获取订单的发货记录
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.SalesManage.Order.Self)]
        public async Task<List<ShipmentDto>> GetOrderShipments(long orderId)
        {
            List<ShipmentDto> shipmentDtoList = new List<ShipmentDto>();
            var shipments = _shipmentManager.FindByOrderId(orderId);

            if (shipments.IsNullOrEmpty())
                return shipmentDtoList;

            foreach (var shipment in shipments)
            {
                var shipmentDto = ObjectMapper.Map<ShipmentDto>(shipment);
                shipmentDto.Items = new List<ShipmentItemDto>();

                var order = await _orderManager.GetByIdAsync(shipment.OrderId);

                shipmentDto.DeliveryOn = shipment.CreationTime;
                shipmentDto.ShippingName = order.ShippingName;
                shipmentDto.ShippingPhoneNumber = order.ShippingPhoneNumber;
                shipmentDto.ShippingAddress = order.GetFullShippingAddress();
                shipmentDto.StatusString = shipment.Status.GetLocalizedEnum(_localizationManager);
                if (shipment.LogisticsId.HasValue)
                {
                    var tenantLogistics = await _logisticsManager.FindTenantLogisticsByLogisticsIdAsync(shipment.LogisticsId.Value);
                    if (tenantLogistics == null)
                    {
                        tenantLogistics = new TenantLogistics()
                        {
                            LogisticsId = shipment.LogisticsId.Value,
                            Name = shipment.LogisticsName,
                        };
                        await _logisticsManager.CreateTenantLogisticsAsync(tenantLogistics);

                        await CurrentUnitOfWork.SaveChangesAsync();
                    }
                    shipmentDto.LogisticsId = tenantLogistics.Id;
                }

                foreach (var item in shipment.Items)
                {
                    shipmentDto.Items.Add(await PrepareShipmentItemDto(item));
                }

                shipmentDtoList.Add(shipmentDto);
            }

            return shipmentDtoList;
        }

        /// <summary>
        /// 获取发货记录详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.SalesManage.Shipment.Self)]
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
        [AbpAuthorize(BusinessCenterPermissions.SalesManage.Shipment.Self)]
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

        #region Method

        /// <summary>
        /// 快速发货
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.SalesManage.Order.Self)]
        public async Task QuickDelivery(QuickDeliveryInput input)
        {
            var order = await _orderManager.GetByIdAsync(input.OrderId);
            if (order == null)
                throw new UserFriendlyException("无效的订单");

            await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Items);
            await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Shipments);

            Shipment shipment = null;
            decimal totalWeight = 0;
            decimal totalVolume = 0;
            foreach (var orderItem in order.Items.ToList())
            {
                //是否还有订单项需要发货
                var maxQtyToAdd = orderItem.GetTotalNumberOfItemsCanBeAddedToShipment(_shipmentManager);
                if (maxQtyToAdd <= 0)
                    continue;

                int qtyToAdd = orderItem.Quantity; //默认

                //validate quantity
                if (qtyToAdd <= 0)
                    continue;
                if (qtyToAdd > maxQtyToAdd)
                    qtyToAdd = maxQtyToAdd;

                var orderItemTotalWeight = orderItem.Weight > 0 ? orderItem.Weight * qtyToAdd : 0;
                totalWeight += orderItemTotalWeight;

                var orderItemTotalVolume = orderItem.Volume > 0 ? orderItem.Volume * qtyToAdd : 0;
                totalVolume += orderItemTotalVolume;

                if (shipment == null)
                {
                    var logistics = await _logisticsManager.FindTenantLogisticsByIdAsync(input.LogisticsId);
                    shipment = new Shipment()
                    {
                        OrderId = order.Id,
                        OrderNumber = order.OrderNumber,
                        LogisticsName = logistics.Name,
                        LogisticsId = logistics.LogisticsId,
                        LogisticsNumber = input.LogisticsNumber,
                        TotalWeight = orderItemTotalWeight,
                        TotalVolume = orderItemTotalVolume,
                        Status = ShippingStatus.NoTrace,
                        AdminComment = input.AdminComment,
                        Items = new Collection<ShipmentItem>()
                    };
                }
                //create a shipment item
                var shipmentItem = new ShipmentItem()
                {
                    OrderItemId = orderItem.Id,
                    Quantity = qtyToAdd,
                };
                shipment.Items.Add(shipmentItem);
            }

            //if we have at least one item in the shipment, then save it
            if (shipment != null && shipment.Items.Count > 0)
            {
                shipment.TotalWeight = totalWeight;
                shipment.TotalVolume = totalVolume;

                if (shipment.Id == 0)
                    await _shipmentManager.CreateAsync(shipment);
                else
                    await _shipmentManager.UpdateAsync(shipment);

                //修改状态为已发货
                await _orderProcessingManager.ShipAsync(shipment, true);
            }
        }

        #endregion

        #region Utilities

        private async Task<List<ShipmentListDto>> PrepareShipmentListDto(List<Shipment> shipments)
        {
            var shipmentDtos = new List<ShipmentListDto>();

            foreach (var shipment in shipments)
            {
                var shipmentDto = ObjectMapper.Map<ShipmentListDto>(shipment);

                var order = await _orderManager.GetByIdAsync(shipment.OrderId);

                shipmentDto.DeliveryOn = shipment.CreationTime;
                shipmentDto.ShippingName = order.ShippingName;
                shipmentDto.ShippingPhoneNumber = order.ShippingPhoneNumber;
                shipmentDto.ShippingAddress = order.GetFullShippingAddress();
                shipmentDto.StatusString = shipment.Status.GetLocalizedEnum(_localizationManager);

                shipmentDtos.Add(shipmentDto);
            }

            return shipmentDtos;
        }

        private async Task<ShipmentItemDto> PrepareShipmentItemDto(ShipmentItem item)
        {
            var orderItem = await _orderManager.GetOrderItemByIdAsync(item.OrderItemId);

            var shipmentDto = ObjectMapper.Map<ShipmentItemDto>(item);

            shipmentDto.ProductName = orderItem.ProductName;
            shipmentDto.AttributeInfo = orderItem.AttributeDescription;

            return shipmentDto;
        }

        /// <summary>
        /// 创建物流
        /// </summary>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.SalesManage.Shipment.Create)]
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
        [AbpAuthorize(BusinessCenterPermissions.SalesManage.Shipment.Edit)]
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
