using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.ECommerce.Orders.Dto;
using System.Linq.Dynamic.Core;
using Abp.Localization;
using Abp.Runtime.Caching;
using Vapps.ECommerce.Stores;
using Vapps.ECommerce.Products;
using Vapps.Media;
using Vapps.States;
using Newtonsoft.Json;
using Vapps.ECommerce.Products.Dto;
using System.Collections.ObjectModel;
using Vapps.ECommerce.Shippings;
using Abp.Domain.Repositories;
using Vapps.Dto;
using Vapps.ECommerce.Payments;
using Abp.Domain.Uow;

namespace Vapps.ECommerce.Orders
{
    public class OrderAppService : VappsAppServiceBase, IOrderAppService
    {

        private readonly IOrderManager _orderManager;
        private readonly IProductManager _productManager;
        private readonly IStoreManager _storeManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly IPictureManager _pictureManager;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IStateManager _stateManager;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly ICacheManager _cacheManager; // TODO: 待实现

        public OrderAppService(IOrderManager orderAppService,
            IProductManager productManager,
            IStoreManager storeManager,
            ILocalizationManager localizationManager,
            IPictureManager pictureManager,
            IProductAttributeParser productAttributeParser,
            IStateManager stateManager,
            IProductAttributeFormatter productAttributeFormatter,
            ICacheManager cacheManager)
        {
            this._orderManager = orderAppService;
            this._storeManager = storeManager;
            this._localizationManager = localizationManager;
            this._cacheManager = cacheManager;
            this._pictureManager = pictureManager;
            this._productAttributeParser = productAttributeParser;
            this._stateManager = stateManager;
            this._productAttributeFormatter = productAttributeFormatter;
            this._productManager = productManager;
        }

        #region Method

        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<OrderListDto>> GetOrders(GetOrdersInput input)
        {
            string proviceName = input.ProviceId.HasValue ?
                (await _stateManager.GetProvinceByIdAsync(input.ProviceId.Value)).Name : null;
            string cityName = input.CityId.HasValue ?
                (await _stateManager.GetCityByIdAsync(input.CityId.Value)).Name : null;
            string districtName = input.DistrictId.HasValue ?
               (await _stateManager.FindDistrictByIdAsync(input.DistrictId.Value)).Name : null;

            var query = _orderManager
               .Orderes
               .Include(o => o.Items)
               .Include(o => o.Shipments)
               .WhereIf(input.OrderTypes.IsNullOrEmpty(), r => input.OrderTypes.Contains(r.OrderType))
               .WhereIf(input.OrderSource.IsNullOrEmpty(), r => input.OrderSource.Contains(r.OrderSource))
               .WhereIf(input.OrderStatuses.IsNullOrEmpty(), r => input.OrderStatuses.Contains(r.OrderStatus))
               .WhereIf(input.ShippingStatuses.IsNullOrEmpty(), r => input.ShippingStatuses.Contains(r.ShippingStatus))
               .WhereIf(input.PaymentStatuses.IsNullOrEmpty(), r => input.PaymentStatuses.Contains(r.PaymentStatus))
               .WhereIf(!input.OrderNumber.IsNullOrWhiteSpace(), r => r.OrderNumber.Contains(input.OrderNumber))
               .WhereIf(input.ProductIds != null && input.ProductIds.Any(), r => r.Items.Any(i => input.ProductIds.Contains(i.ProductId)))
               .WhereIf(input.LogisticsNumber.IsNullOrWhiteSpace(), r => r.Shipments.Any(s => s.LogisticsNumber.Contains(input.LogisticsNumber)))
               .WhereIf(input.ShippingName.IsNullOrWhiteSpace(), r => r.ShippingName.Contains(input.ShippingName))
               .WhereIf(input.PhoneNumber.IsNullOrWhiteSpace(), r => r.ShippingPhoneNumber.Contains(input.PhoneNumber))
               .WhereIf(proviceName.IsNullOrWhiteSpace(), r => r.ShippingProvice.Contains(proviceName))
               .WhereIf(cityName.IsNullOrWhiteSpace(), r => r.ShippingCity.Contains(cityName))
               .WhereIf(districtName.IsNullOrWhiteSpace(), r => r.ShippingDistrict.Contains(districtName));

            var orderCount = await query.CountAsync();

            var orders = await query
                .OrderBy(input.Sorting)
                .PageBy(input)
                .ToListAsync();

            var orderListDtos = orders.Select(o =>
            {
                var orderDto = PrepareOrderListDto(o).Result;
                return orderDto;
            }).ToList();

            return new PagedResultDto<OrderListDto>(
                orderCount,
                orderListDtos);
        }

        /// <summary>
        /// 获取订单详情
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<OrderDetailDto> GetOrderDetail(long orderId)
        {
            var order = await _orderManager.GetByIdAsync(orderId);

            await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Items);

            return await PrepareOrderDetailDto(order);
        }

        /// <summary>
        /// 创建或更新订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<EntityDto<long>> CreateOrUpdateOrder(CreateOrUpdateOrderInput input)
        {
            if (input.Id > 0)
            {
                await UpdateOrderAsync(input);

                return new EntityDto<long>() { Id = input.Id };
            }
            else
            {
                return await CreateOrderAsync(input);
            }
        }

        /// <summary>
        /// 修改订单状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ChangeOrderStatus(ChangeOrderStatusInput<OrderStatus> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                var order = await _orderManager.GetByIdAsync(input.Id);

                order.OrderStatus = input.Stauts;

                await _orderManager.UpdateAsync(order);
            }
        }

        /// <summary>
        /// 修改物流状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task ChangeShippingStatus(ChangeOrderStatusInput<ShippingStatus> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                var order = await _orderManager.GetByIdAsync(input.Id);

                order.ShippingStatus = input.Stauts;

                await _orderManager.UpdateAsync(order);
            }
        }

        /// <summary>
        /// 修改付款状态
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [UnitOfWork(isTransactional: false)]
        public async Task ChangePaymentStatus(ChangeOrderStatusInput<PaymentStatus> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                var order = await _orderManager.GetByIdAsync(input.Id);

                order.PaymentStatus = input.Stauts;

                await _orderManager.UpdateAsync(order);
            }
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task DeleteOrder(BatchInput<long> input)
        {
            if (input.Ids == null || input.Ids.Count() <= 0)
            {
                return;
            }

            foreach (var id in input.Ids)
            {
                await _orderManager.DeleteAsync(id);
            }
        }

        #endregion

        #region Utility

        private async Task<EntityDto<long>> CreateOrderAsync(CreateOrUpdateOrderInput input)
        {
            var order = ObjectMapper.Map<Order>(input);

            order.GenerateOrderNumber();

            await UpdateAddressInfo(input, order);

            await CreateOrUpdateOrderItem(input, order);

            await _orderManager.CreateAsync(order);

            return new EntityDto<long>(order.Id);
        }

        private async Task<EntityDto<long>> UpdateOrderAsync(CreateOrUpdateOrderInput input)
        {
            var order = await _orderManager.GetByIdAsync(input.Id);

            await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Items);

            ObjectMapper.Map(input, order);

            if (order.OrderNumber.IsNullOrWhiteSpace())
            {
                order.GenerateOrderNumber();
            }

            await UpdateAddressInfo(input, order);

            await CreateOrUpdateOrderItem(input, order);

            await _orderManager.UpdateAsync(order);

            return new EntityDto<long>(order.Id);
        }

        /// <summary>
        /// 创建或更新子订单
        /// </summary>
        /// <param name="input"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private async Task CreateOrUpdateOrderItem(CreateOrUpdateOrderInput input, Order order)
        {
            if (input.Id == 0)
            {
                order.Items = new Collection<OrderItem>();
            }
            else
            {
                var existItemIds = input.Items.Select(i => i.Id).ToList();
                var itemsId2Remove = order.Items.Where(i => !existItemIds.Contains(i.Id)).ToList();

                //删除不存在的属性
                foreach (var item in itemsId2Remove)
                {
                    item.IsDeleted = true;
                    order.Items.Remove(item);
                }
            }

            foreach (var oderItemDto in input.Items)
            {
                var orderItem = ObjectMapper.Map<OrderItem>(oderItemDto);

                if (oderItemDto.Id > 0)
                {
                    orderItem = order.Items.FirstOrDefault(oi => oi.Id == oderItemDto.Id);
                    ObjectMapper.Map(oderItemDto, orderItem);
                }
                else
                {
                    orderItem = ObjectMapper.Map<OrderItem>(oderItemDto); ;
                }

                var product = await _productManager.GetByIdAsync(oderItemDto.ProductId);
                var attributesJson = JsonConvert.SerializeObject(oderItemDto.Attributes.GetAttributesJson(product));

                if (!oderItemDto.Attributes.IsNullOrEmpty())
                {
                    var combin = product.AttributeCombinations.FirstOrDefault(c => c.AttributesJson == attributesJson);
                    if (combin != null)
                    {
                        orderItem.AttributesJson = attributesJson;
                        orderItem.AttributeDescription = _productAttributeFormatter.FormatAttributes(product, attributesJson);
                    }
                }

                if (orderItem.Id <= 0)
                    order.Items.Add(orderItem);
            }
        }

        private async Task UpdateAddressInfo(CreateOrUpdateOrderInput input, Order order)
        {
            var province = await _stateManager.GetProvinceByIdAsync(input.ShippingProviceId);
            var city = await _stateManager.GetCityByIdAsync(input.ShippingCityId);
            var district = await _stateManager.GetDistrictByIdAsync(input.ShippingDistrictId);

            order.ShippingProvice = province?.Name ?? string.Empty;
            order.ShippingCity = city?.Name ?? string.Empty;
            order.ShippingDistrict = district?.Name ?? string.Empty;
        }

        private async Task<OrderListDto> PrepareOrderListDto(Order order)
        {
            var store = await _storeManager.GetByIdAsync(order.StoreId);
            OrderListDto dto = new OrderListDto()
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderStatus = order.OrderStatus,
                OrderStatusString = order.OrderStatus.GetLocalizedEnum(_localizationManager),
                ShippingStatus = order.ShippingStatus,
                ShippingStatusString = order.ShippingStatus.GetLocalizedEnum(_localizationManager),
                PaymentStatus = order.PaymentStatus,
                PaymentStatusString = order.PaymentStatus.GetLocalizedEnum(_localizationManager),
                OrderSource = order.OrderSource,
                OrderSourceString = order.OrderSource.GetLocalizedEnum(_localizationManager),
                OrderTypeString = order.OrderType.GetLocalizedEnum(_localizationManager),
                OrderType = order.OrderType,
                AdminComment = order.AdminComment,
                CustomerComment = order.CustomerComment,
                Store = store?.Name ?? string.Empty,
                TotalAmount = order.TotalAmount,
                CreateOn = order.CreationTime,
            };

            foreach (var item in order.Items)
            {
                var product = await _productManager.GetByIdAsync(item.ProductId);
                var itemDto = new OrderListItemDto()
                {
                    Id = item.Id,
                    UnitPrice = item.UnitPrice,
                    Price = item.Price,
                    PictureUrl = await product.GetProductDefaultPictureUrl(item.AttributesJson, _pictureManager, _productAttributeParser),
                    ProductName = GetOrderProductDescription(item)
                };

                dto.Items.Add(itemDto);
            }

            return dto;
        }

        private async Task<OrderDetailDto> PrepareOrderDetailDto(Order order)
        {
            var store = await _storeManager.GetByIdAsync(order.StoreId);

            var dto = ObjectMapper.Map<OrderDetailDto>(order);
            dto.OrderTypeString = order.OrderType.GetLocalizedEnum(_localizationManager);
            dto.OrderStatusString = order.OrderStatus.GetLocalizedEnum(_localizationManager);
            dto.ShippingStatusString = order.ShippingStatus.GetLocalizedEnum(_localizationManager);
            dto.PaymentStatusString = order.PaymentStatus.GetLocalizedEnum(_localizationManager);
            dto.OrderSourceString = order.OrderSource.GetLocalizedEnum(_localizationManager);
            dto.Store = store.Name;

            var province = await _stateManager.FindProvinceByNameAsync(order.ShippingProvice);
            var city = await _stateManager.FindCityByNameAsync(order.ShippingCity);
            var district = await _stateManager.FindDistrictByNameAsync(order.ShippingDistrict);

            dto.ShippingProviceId = province?.Id ?? 0;
            dto.ShippingCityId = city?.Id ?? 0;
            dto.ShippingDistrictId = district?.Id ?? 0;

            foreach (var item in order.Items)
            {
                var product = await _productManager.GetByIdAsync(item.ProductId);
                var itemDto = ObjectMapper.Map<OrderDetailItemDto>(item);
                itemDto.PictureUrl = await product.GetProductDefaultPictureUrl(item.AttributesJson, _pictureManager, _productAttributeParser);
                itemDto.ProductName = GetOrderProductDescription(item);

                dto.Items.Add(itemDto);
            }

            return dto;
        }

        private static string GetOrderProductDescription(OrderItem item)
        {
            if (item.AttributeDescription.IsNullOrEmpty())
                return $"{item.ProductName}";
            else
                return $"{item.ProductName} - {item.AttributeDescription}";
        }

        #endregion
    }
}
