using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.Localization;
using Abp.Runtime.Caching;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Dto;
using Vapps.ECommerce.Orders.Dto;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Products.Dto;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Stores;
using Vapps.Media;
using Vapps.States;

namespace Vapps.ECommerce.Orders
{
    [AbpAuthorize(BusinessCenterPermissions.Order.Self)]
    public class OrderAppService : VappsAppServiceBase, IOrderAppService
    {

        private readonly IOrderManager _orderManager;
        private readonly IProductManager _productManager;
        private readonly IProductAttributeManager _productAttributeManager;
        private readonly IStoreManager _storeManager;
        private readonly ILocalizationManager _localizationManager;
        private readonly IPictureManager _pictureManager;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IStateManager _stateManager;
        private readonly IProductAttributeFormatter _productAttributeFormatter;

        private readonly ICacheManager _cacheManager; // TODO: 待实现

        public OrderAppService(IOrderManager orderAppService,
            IProductManager productManager,
            IProductAttributeManager productAttributeManager,
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
            this._productAttributeManager = productAttributeManager;
        }

        #region Method

        /// <summary>
        /// 获取所有订单
        /// </summary>
        /// <returns></returns>
        public async Task<PagedResultDto<OrderListDto>> GetOrders(GetOrdersInput input)
        {
            string provinceName = input.ProvinceId.HasValue ?
                (await _stateManager.GetProvinceByIdAsync(input.ProvinceId.Value)).Name : null;
            string cityName = input.CityId.HasValue ?
                (await _stateManager.GetCityByIdAsync(input.CityId.Value)).Name : null;
            string districtName = input.DistrictId.HasValue ?
               (await _stateManager.FindDistrictByIdAsync(input.DistrictId.Value)).Name : null;

            var query = _orderManager
               .Orderes
               .Include(o => o.Items)
               .Include(o => o.Shipments)
               .WhereIf(!input.OrderTypes.IsNullOrEmpty(), r => input.OrderTypes.Contains(r.OrderType))
               .WhereIf(!input.OrderSources.IsNullOrEmpty(), r => input.OrderSources.Contains(r.OrderSource))
               .WhereIf(!input.OrderStatuses.IsNullOrEmpty(), r => input.OrderStatuses.Contains(r.OrderStatus))
               .WhereIf(!input.ShippingStatuses.IsNullOrEmpty(), r => input.ShippingStatuses.Contains(r.ShippingStatus))
               .WhereIf(!input.PaymentStatuses.IsNullOrEmpty(), r => input.PaymentStatuses.Contains(r.PaymentStatus))
               .WhereIf(!input.OrderNumber.IsNullOrWhiteSpace(), r => r.OrderNumber.Contains(input.OrderNumber))
               .WhereIf(input.ProductIds != null && input.ProductIds.Any(), r => r.Items.Any(i => input.ProductIds.Contains(i.ProductId)))
               .WhereIf(!input.LogisticsNumber.IsNullOrWhiteSpace(), r => r.Shipments.Any(s => s.LogisticsNumber.Contains(input.LogisticsNumber)))
               .WhereIf(!input.ShippingName.IsNullOrWhiteSpace(), r => r.ShippingName.Contains(input.ShippingName))
               .WhereIf(!input.PhoneNumber.IsNullOrWhiteSpace(), r => r.ShippingPhoneNumber.Contains(input.PhoneNumber))
               .WhereIf(!provinceName.IsNullOrWhiteSpace(), r => r.ShippingProvince.Contains(provinceName))
               .WhereIf(!cityName.IsNullOrWhiteSpace(), r => r.ShippingCity.Contains(cityName))
               .WhereIf(!districtName.IsNullOrWhiteSpace(), r => r.ShippingDistrict.Contains(districtName));

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
        /// 获取订单详情(包含商品属性)
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        public async Task<GetOrderForEditOutput> GetOrderForEdit(long orderId)
        {
            var order = await _orderManager.GetByIdAsync(orderId);

            await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Items);

            return await PrepareOrderForEditOutput(order);
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
        [UnitOfWork(isTransactional: false)]
        [AbpAuthorize(BusinessCenterPermissions.Order.Edit)]
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
        [UnitOfWork(isTransactional: false)]
        [AbpAuthorize(BusinessCenterPermissions.Order.Edit)]
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
        [AbpAuthorize(BusinessCenterPermissions.Order.Edit)]
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
        [AbpAuthorize(BusinessCenterPermissions.Order.Delete)]
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

        /// <summary>
        /// 创建订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.Order.Create)]
        private async Task<EntityDto<long>> CreateOrderAsync(CreateOrUpdateOrderInput input)
        {
            var order = ObjectMapper.Map<Order>(input);

            order.GenerateOrderNumber();

            InitOrderAmount(input, order);

            await InitOrderEnumField(input, order);

            await UpdateAddressInfo(input, order);

            await CreateOrUpdateOrderItem(input, order);

            await _orderManager.CreateAsync(order);

            await CurrentUnitOfWork.SaveChangesAsync();

            return new EntityDto<long>(order.Id);
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(BusinessCenterPermissions.Order.Edit)]
        private async Task<EntityDto<long>> UpdateOrderAsync(CreateOrUpdateOrderInput input)
        {
            var order = await _orderManager.GetByIdAsync(input.Id);

            await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Items);

            ObjectMapper.Map(input, order);

            if (order.OrderNumber.IsNullOrWhiteSpace())
            {
                order.GenerateOrderNumber();
            }

            InitOrderAmount(input, order);

            await InitOrderEnumField(input, order);

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
                OrderItem orderItem = null;

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
                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.Attributes);
                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, t => t.AttributeCombinations);

                var jsonAttributeList = oderItemDto.Attributes.GetAttributesJson(product, _productAttributeManager);
                var jsonAttributesString = JsonConvert.SerializeObject(jsonAttributeList);

                orderItem.ProductName = product.Name;
                orderItem.Weight = product.Weight;
                orderItem.Volume = product.GetVolume();
                if (!oderItemDto.Attributes.IsNullOrEmpty())
                {
                    var combin = product.AttributeCombinations.FirstOrDefault(c => c.AttributesJson == jsonAttributesString);

                    if (combin != null)
                    {
                        orderItem.AttributesJson = jsonAttributesString;
                        orderItem.AttributeDescription = await _productAttributeFormatter.FormatAttributesAsync(product, jsonAttributeList);
                    }
                }

                if (orderItem.OrderItemNumber.IsNullOrWhiteSpace())
                {
                    orderItem.GenerateOrderItemNumber();
                }
                if (orderItem.Id <= 0)
                    order.Items.Add(orderItem);

                order.TotalAmount += product.Price;
                order.SubtotalAmount += product.Price;
            }
        }

        /// <summary>
        /// 初始化枚举字段
        /// </summary>
        /// <param name="input"></param>
        /// <param name="order"></param>
        private async Task InitOrderEnumField(CreateOrUpdateOrderInput input, Order order)
        {
            order.OrderType = input.OrderType.HasValue ? input.OrderType.Value : OrderType.PayOnDelivery;
            order.OrderSource = input.OrderSource.HasValue ? input.OrderSource.Value : OrderSource.Self;
            order.OrderStatus = input.OrderStatus.HasValue ? input.OrderStatus.Value : OrderStatus.WaitConfirm;
            order.PaymentStatus = input.PaymentStatus.HasValue ? input.PaymentStatus.Value : PaymentStatus.Pending;
            order.ShippingStatus = input.ShippingStatus.HasValue ? input.ShippingStatus.Value : ShippingStatus.NotYetShipped;
            order.PaymentType = order.OrderType == OrderType.PayOnDelivery ? PaymentType.PayOnDelivery : PaymentType.PayOnline;

            if (input.OrderSource.HasValue)
            {
                order.OrderSource = input.OrderSource.Value;
            }
            else
            {
                var store = await _storeManager.GetByIdAsync(input.StoreId);

                if (store != null)
                {
                    order.OrderSource = store.OrderSource;
                }
                else
                {
                    order.OrderSource = OrderSource.Self;
                }
            }
        }

        /// <summary>
        /// 初始订单金额
        /// </summary>
        /// <param name="input"></param>
        /// <param name="order"></param>
        private void InitOrderAmount(CreateOrUpdateOrderInput input, Order order)
        {
            order.DiscountAmount = input.DiscountAmount ?? 0;
            order.RefundedAmount = input.RefundedAmount ?? 0;
            order.RewardAmount = input.RewardAmount ?? 0;
            order.ShippingAmount = input.ShippingAmount ?? 0; ;
            order.SubtotalAmount = input.SubtotalAmount ?? 0;
            order.SubTotalDiscountAmount = input.SubTotalDiscountAmount ?? 0;
            order.TotalAmount = input.TotalAmount ?? 0;
            order.PaymentMethodAdditionalFee = input.PaymentMethodAdditionalFee ?? 0;
        }

        /// <summary>
        /// 更新地址信息
        /// </summary>
        /// <param name="input"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        private async Task UpdateAddressInfo(CreateOrUpdateOrderInput input, Order order)
        {
            var province = await _stateManager.GetProvinceByIdAsync(input.ShippingProvinceId);
            var city = await _stateManager.GetCityByIdAsync(input.ShippingCityId);
            var district = await _stateManager.GetDistrictByIdAsync(input.ShippingDistrictId);

            order.ShippingProvince = province?.Name ?? string.Empty;
            order.ShippingCity = city?.Name ?? string.Empty;
            order.ShippingDistrict = district?.Name ?? string.Empty;
        }

        /// <summary>
        /// 初始化订单列表Dto
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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

                ShippingName = order.ShippingName,
                ShippingPhoneNumber = order.ShippingPhoneNumber,
                ShippingAddress = $"{order.ShippingProvince}{order.ShippingCity}{order.ShippingDistrict}{order.ShippingAddress}"
            };

            foreach (var item in order.Items)
            {
                var product = await _productManager.GetByIdAsync(item.ProductId);

                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, p => p.Pictures);

                var itemDto = new OrderListItemDto()
                {
                    Id = item.Id,
                    UnitPrice = item.UnitPrice,
                    Price = item.Price,
                    PictureUrl = await product.GetProductDefaultPictureUrl(item.AttributesJson, _pictureManager, _productAttributeParser),
                    ProductName = product.Name,
                    Quantity = item.Quantity,
                    AttributeDesciption = GetOrderProductDescription(item)
                };

                dto.Items.Add(itemDto);
            }

            return dto;
        }

        /// <summary>
        /// 初始化订单详情Dto
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
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

            var province = await _stateManager.FindProvinceByNameAsync(order.ShippingProvince);
            var city = await _stateManager.FindCityByNameAsync(order.ShippingCity);
            var district = await _stateManager.FindDistrictByNameAsync(order.ShippingDistrict);

            dto.ShippingProvinceId = province?.Id ?? 0;
            dto.ShippingCityId = city?.Id ?? 0;
            dto.ShippingDistrictId = district?.Id ?? 0;

            foreach (var item in order.Items)
            {
                var product = await _productManager.GetByIdAsync(item.ProductId);

                await _productManager.ProductRepository.EnsureCollectionLoadedAsync(product, p => p.Pictures);

                var itemDto = ObjectMapper.Map<OrderDetailItemDto>(item);

                itemDto.PictureUrl = await product.GetProductDefaultPictureUrl(item.AttributesJson, _pictureManager, _productAttributeParser);
                itemDto.AttributeDesciption = GetOrderProductDescription(item);
                itemDto.ProductName = product.Name;
                dto.Items.Add(itemDto);
            }

            return dto;
        }

        /// <summary>
        /// 初始化订单编辑Dto
        /// </summary>
        /// <param name="order"></param>
        /// <returns></returns>
        private async Task<GetOrderForEditOutput> PrepareOrderForEditOutput(Order order)
        {
            var dto = ObjectMapper.Map<GetOrderForEditOutput>(order);

            var province = await _stateManager.FindProvinceByNameAsync(order.ShippingProvince);
            var city = await _stateManager.FindCityByNameAsync(order.ShippingCity);
            var district = await _stateManager.FindDistrictByNameAsync(order.ShippingDistrict);

            dto.ShippingProvinceId = province?.Id ?? 0;
            dto.ShippingCityId = city?.Id ?? 0;
            dto.ShippingDistrictId = district?.Id ?? 0;

            foreach (var item in order.Items)
            {
                var product = await _productManager.GetByIdAsync(item.ProductId);
                var itemDto = ObjectMapper.Map<OrderItemDto>(item);

                itemDto.Attributes = await PrepareOrderItemAttribute(item);

                dto.Items.Add(itemDto);
            }

            return dto;
        }

        /// <summary>
        /// 初始化子订单商品属性
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private async Task<List<ProductAttributeDto>> PrepareOrderItemAttribute(OrderItem item)
        {
            var attributes = new List<ProductAttributeDto>();

            if (item.AttributesJson.IsNullOrWhiteSpace())
                return attributes;

            var jsonAttributeList = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(item.AttributesJson);

            foreach (var jsonAttribute in jsonAttributeList)
            {
                var atributeDto = new ProductAttributeDto();

                var attribute = await _productAttributeManager.GetByIdAsync(jsonAttribute.AttributeId);

                atributeDto.Id = jsonAttribute.AttributeId;
                atributeDto.Name = attribute.Name;
                atributeDto.Values = jsonAttribute.AttributeValues.Select(value =>
                {
                    var valueDto = new ProductAttributeValueDto();

                    var attributeValue = _productAttributeManager.FindValueById(value.AttributeValueId);
                    if (attributeValue == null)
                        valueDto.Name = _productAttributeManager.GetPredefinedValueById(value.AttributeValueId).Name;
                    else
                        valueDto.Name = attributeValue.Name;

                    valueDto.Id = attributeValue.Id;
                    valueDto.PictureUrl = _pictureManager.GetPictureUrl(attributeValue.PictureId);
                    return valueDto;
                }).ToList();
                attributes.Add(atributeDto);
            }

            return attributes;
        }

        /// <summary>
        /// 获取商品描述
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        private static string GetOrderProductDescription(OrderItem item)
        {
            if (item.AttributeDescription.IsNullOrEmpty())
                return string.Empty;
            else
                return item.AttributeDescription;
        }

        #endregion
    }
}
