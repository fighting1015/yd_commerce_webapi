using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Extensions;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Addresses;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Prices;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Stores;
using Vapps.States;
using Vapps.States.Cache;

namespace Vapps.ECommerce.Orders
{
    public class OrderImportor : VappsDomainServiceBase, IOrderImportor
    {
        #region Ctor

        private readonly IStoreManager _storeManager;
        private readonly IOrderManager _orderManager;
        private readonly IShipmentManager _shipmentManager;
        private readonly IProductManager _productManager;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IProductAttributeManager _productAttributeManager;
        private readonly ILogisticsManager _logisticsManager;
        private readonly ILogger _logger;
        private readonly IPriceCalculator _priceCalculator;
        private readonly IStateManager _stateManager;
        private readonly IAddressManager _addressManager;
        private readonly IStateCache _stateCache;

        public OrderImportor(IOrderManager orderManager,
            IStoreManager storeManager,
            IProductManager productManager,
            IShipmentManager shipmentManager,
            IProductAttributeFormatter productAttributeFormatter,
            IProductAttributeManager productAttributeManager,
            ILogisticsManager logisticsManager,
            ILogger logger,
            IPriceCalculator priceCalculator,
            IStateManager stateManager,
            IAddressManager addressManager,
            IStateCache stateCache)
        {
            this._orderManager = orderManager;
            this._storeManager = storeManager;
            this._productManager = productManager;
            this._shipmentManager = shipmentManager;
            this._productAttributeFormatter = productAttributeFormatter;
            this._productAttributeManager = productAttributeManager;
            this._logisticsManager = logisticsManager;
            this._logger = logger;
            this._stateManager = stateManager;
            this._addressManager = addressManager;
            this._stateCache = stateCache;
            this._priceCalculator = priceCalculator;
        }

        #endregion

        #region Order Import

        /// <summary>
        /// 导入订单
        /// </summary>
        /// <param name="orderImport"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task<bool> ImportOrderAsync(OrderImport orderImport)
        {


            //跳过已取消订单
            if (orderImport.OrderStatus == OrderStatus.Canceled
                && orderImport.ShipName.IsNullOrWhiteSpace())
            {
                return false;
            }

            if (!orderImport.CustomerComment.IsNullOrWhiteSpace())
            {
                if (orderImport.CustomerComment.Trim().ToLower() == "x" ||
                orderImport.CustomerComment.Trim() == "shua")

                    return false;
            }

            if (!orderImport.AdminComment.IsNullOrWhiteSpace())
            {
                if (orderImport.AdminComment.Trim().ToLower() == "x" ||
                orderImport.AdminComment.Trim() == "shua")

                    return false;
            }

            var orderItemImportList = await GetOrderItem(orderImport.ProductSku, orderImport.OrderNumber);
            if (!orderItemImportList.Any())
            {
                return false;
            }

            var order = await _orderManager.FindByOrderNumberAsync(orderImport.OrderNumber);

            if (order != null)
            {
                //await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Items);
                //await _orderManager.OrderRepository.EnsureCollectionLoadedAsync(order, o => o.Shipments);

                //if (order.Shipments.Count == 0 || order.ShippingStatus == ShippingStatus.NotYetShipped)
                //{
                //    await AddShipment(orderImport, order);
                //    await _orderManager.UpdateAsync(order);
                //}
                //else
                //{
                //    order.Shipments.FirstOrDefault().LogisticsNumber = orderImport.TrackingNumber;
                //}

                //order.RewardAmount = orderImport.Reward;
                //order.CreationTime = orderImport.CreatedOnUtc;

                //await _orderManager.UpdateAsync(order);

                return true;
            }

            var address = await PrepareShippingAddress(orderImport);
            order = new Order()
            {
                OrderSource = orderImport.OrderSource,
                StoreId = orderImport.StoreId > 0 ? orderImport.StoreId : _storeManager.Stores.FirstOrDefault()?.Id ?? 0,
                OrderNumber = orderImport.OrderNumber,
                UserId = 0, // TODO:创建用户
                SubtotalAmount = orderImport.OrderTotal,
                TotalAmount = orderImport.OrderTotal,
                DiscountAmount = orderImport.DiscountAmount,
                OrderStatus = orderImport.OrderStatus,
                PaymentStatus = PaymentStatus.Pending,
                ShippingName = address.FullName,
                ShippingProvince = address.Province,
                ShippingCity = address.City,
                ShippingDistrict = address.District,
                ShippingPhoneNumber = address.PhoneNumber,
                ShippingAddress = address.DetailAddress,
                ShippingStatus = ShippingStatus.NotYetShipped,

                CreationTime = orderImport.CreatedOnUtc,
                OrderType = OrderType.PayOnDelivery,
                AdminComment = orderImport.AdminComment,
                CustomerComment = orderImport.CustomerComment,
                RewardAmount = orderImport.Reward,
                Items = new Collection<OrderItem>(),
                Shipments = new Collection<Shipment>()
            };

            //插入新订单
            await _orderManager.CreateAsync(order);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            foreach (var orderItemImport in orderItemImportList)
            {
                var price = await _priceCalculator.GetProductPriceAsync(orderItemImport.Product, orderItemImport.Combin?.AttributesJson ?? null);
                var cost = await _priceCalculator.GetProductCostAsync(orderItemImport.Product, orderItemImport.Combin?.AttributesJson ?? null);
                //save item
                var orderItem = new OrderItem()
                {
                    OrderItemNumber = orderImport.OrderNumber,
                    ProductId = orderItemImport.Product.Id,
                    Price = orderItemImport.Product.Price,
                    UnitPrice = price,
                    OriginalProductCost = cost,
                    Quantity = orderItemImport.Quantity * orderImport.PackageNum,
                    ProductName = orderItemImport.Product.Name,
                    Weight = orderItemImport.Product.Weight,
                    Volume = orderItemImport.Product.GetVolume(),
                };

                if (orderItemImport.IsCombin)
                {
                    var jsonAttributeList = JsonConvert.DeserializeObject<List<JsonProductAttribute>>(orderItemImport.Combin?.AttributesJson);

                    orderItem.AttributesJson = orderItemImport.Combin?.AttributesJson ?? null;
                    orderItem.AttributeDescription = await _productAttributeFormatter.FormatAttributesAsync(orderItemImport.Product, jsonAttributeList);
                }

                order.Items.Add(orderItem);
            }

            await AddShipment(orderImport, order);

            await _orderManager.UpdateAsync(order);

            await UnitOfWorkManager.Current.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// 获取订单产品信息
        /// </summary>
        /// <param name="productSku"></param>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
        [UnitOfWork]
        private async Task<List<OrderItemImport>> GetOrderItem(string productSku, string orderNumber)
        {
            var orderItemList = new List<OrderItemImport>();

            try
            {
                var itemArray = productSku.Split(',');
                if (!itemArray.Any())
                {
                    _logger.Error($"订单号:{orderNumber}sku不存在，导入失败");
                    return orderItemList;
                }

                for (int i = 0; i < itemArray.Length; i++)
                {
                    var item = itemArray[i].Split('/');
                    var orderItem = new OrderItemImport();
                    var product = await _productManager.FindBySkuAsync(item[0]);
                    if (product != null)
                    {
                        orderItem.IsCombin = false;
                        orderItem.Product = product;
                        orderItem.Quantity = Convert.ToInt32(item[1]);
                    }
                    else
                    {
                        var combin = await _productAttributeManager.FindCombinationBySkuAsync(item[0]);

                        if (combin == null)
                        {
                            _logger.Error($"订单号:{orderNumber}sku不存在，导入失败");
                            return orderItemList;
                        }
                        else
                        {
                            await _productAttributeManager.ProductAttributeCombinationRepository
                            .EnsurePropertyLoadedAsync(combin, c => c.Product);

                            orderItem.IsCombin = true;
                            orderItem.Product = combin.Product;
                            orderItem.Combin = combin;
                            orderItem.Quantity = Convert.ToInt32(item[1]);
                        }
                    }

                    orderItemList.Add(orderItem);
                }
            }
            catch (Exception ex)
            {
                _logger.Error($"订单号:{orderNumber}sku不存在，导入失败");
                return orderItemList;
            }

            return orderItemList;
        }

        /// <summary>
        /// 添加订单信息
        /// </summary>
        /// <param name="orderImport"></param>
        /// <param name="order"></param>
        private async Task<Shipment> AddShipment(OrderImport orderImport, Order order)
        {
            if (orderImport.ShipName.IsNullOrWhiteSpace())
                return null;

            if (orderImport.TrackingNumber.IsNullOrWhiteSpace())
                return null;

            order.ShippingStatus = ShippingStatus.Taked;
            var trackingNumber = orderImport.TrackingNumber.Replace("'", "");

            var logistics = await _logisticsManager.FindByNameAsync(orderImport.ShipName);
            if (logistics == null)
                return null;

            Shipment shipment;
            decimal totalVolume = 0;
            decimal totalWeight = 0;
            if (order.Shipments != null && order.Shipments.Any())
            {
                shipment = order.Shipments.FirstOrDefault();

                await _shipmentManager.ShipmentRepository.EnsureCollectionLoadedAsync(shipment, s => s.Items);

                shipment.LogisticsId = logistics.Id;
                //shipment.TrackingNumber = trackingNumber;
                shipment.CreationTime = orderImport.CreatedOnUtc;
            }
            else
            {
                shipment = new Shipment()
                {
                    LogisticsName = logistics.Name,
                    OrderNumber = order.OrderNumber,
                    Status = order.ShippingStatus,
                    LogisticsId = logistics.Id,
                    OrderId = order.Id,
                    CreationTime = orderImport.CreatedOnUtc,
                    Items = new Collection<ShipmentItem>(),
                };
            }

            shipment.LogisticsNumber = orderImport.TrackingNumber;

            if (orderImport.OrderStatus == OrderStatus.Completed)
            {
                order.OrderStatus = OrderStatus.Completed;
                order.ShippingStatus = ShippingStatus.Received;
                shipment.ReceivedOn = orderImport.DeliveriedOnUtc;
            }

            if (orderImport.OrderStatus == OrderStatus.Canceled)
            {
                order.OrderStatus = OrderStatus.Canceled;
                order.ShippingStatus = ShippingStatus.IssueWithReject;

                shipment.RejectedOn = orderImport.DeliveriedOnUtc;
            }

            if (shipment.Id == 0)
            {
                foreach (var item in order.Items.ToList())
                {
                    shipment.Items.Add(new ShipmentItem()
                    {
                        OrderItemId = item.Id,
                        Quantity = item.Quantity,
                    });

                    totalVolume += item.Volume;
                    totalWeight += item.Weight;

                }

                order.Shipments.Add(shipment);
            }

            shipment.TotalVolume = totalVolume;
            shipment.TotalWeight = totalWeight;

            return shipment;
        }

        /// <summary>
        /// 地址转换
        /// </summary>
        /// <param name="orderImport"></param>
        /// <returns></returns>
        private async Task<Address> PrepareShippingAddress(OrderImport orderImport)
        {
            var address = await _addressManager.FindByPhoneNumerAsync(orderImport.Telephone);
            if (address == null)
            {
                address = new Address();
            }

            var province = _stateCache.GetProvinceByNameOrNull(orderImport.Province);
            address.ProvinceId = province?.Id ?? 0;
            address.Province = province?.Name ?? string.Empty;

            CityCacheItem city;
            List<City> citys = null;
            try
            {
                var test = _stateManager.Cities.Where(c => c.ProvinceId == province.Id);

                citys = await _stateManager.Cities.Where(c => c.ProvinceId == province.Id).ToListAsync();
            }
            catch (Exception ex)
            {

            }

            if (citys.Count() == 1)
            {
                city = _stateCache.GetCityByNameOrNull(citys.FirstOrDefault().Name);
            }
            else
            {
                city = _stateCache.GetCityByNameOrNull(orderImport.City);

                if (city == null)
                {
                    var cityEntity = new City()
                    {
                        ProvinceId = province.Id,
                        Name = orderImport.City,
                        IsActive = true
                    };

                    await _stateManager.CreateCityAsync(cityEntity);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    city = _stateCache.GetCityByNameOrNull(orderImport.City);
                }
            }

            address.CityId = city?.Id ?? 0;
            address.City = city?.Name ?? string.Empty;

            var destrictName = orderImport.District.Replace("　", "");
            if (!destrictName.IsNullOrWhiteSpace())
            {
                var destrict = _stateCache.GetDistrictByNameOrNull(destrictName);
                if (destrict == null)
                {
                    var destrictEntity = new District()
                    {
                        CityId = city.Id,
                        Name = destrictName,
                        IsActive = true
                    };

                    await _stateManager.CreateDistrictAsync(destrictEntity);

                    await CurrentUnitOfWork.SaveChangesAsync();

                    destrict = _stateCache.GetDistrictByNameOrNull(orderImport.City);
                }

                address.DistrictId = destrict?.Id ?? 0;
                address.District = destrict?.Name ?? string.Empty;
            }

            address.DetailAddress = RemoveReplaceAddress(address, orderImport.Address);
            address.PhoneNumber = orderImport.Telephone;
            address.FullName = orderImport.ReceiverName;

            if (address.Id == 0)
                await _addressManager.CreateAsync(address);
            else
                await _addressManager.UpdateAsync(address);

            return address;
        }

        private string RemoveReplaceAddress(Address address, string detailAddress)
        {
            if (!address.Province.IsNullOrWhiteSpace())
            {
                var provinceName = address.Province;

                if (detailAddress.StartsWith(provinceName))
                {
                    detailAddress = detailAddress.Replace(provinceName, "");
                }

                provinceName = provinceName.Substring(0, 2);
                if (detailAddress.StartsWith(provinceName + address.Province))
                {
                    detailAddress = detailAddress.Replace(provinceName + address.Province, "");
                }
                else if (detailAddress.StartsWith(provinceName))
                {
                    detailAddress = detailAddress.Replace(provinceName, "");
                }
            }

            if (!address.City.IsNullOrWhiteSpace())
            {
                var cityName = address.City;

                if (detailAddress.StartsWith(cityName))
                {
                    detailAddress = detailAddress.Replace(cityName, "");
                }

                cityName = cityName.Substring(0, 2);

                if (detailAddress.StartsWith(cityName))
                {
                    detailAddress = detailAddress.Replace(cityName, "");
                }
            }

            if (!address.District.IsNullOrWhiteSpace())
            {
                var districtName = address.District;

                if (detailAddress.StartsWith(districtName))
                {
                    detailAddress = detailAddress.Replace(districtName, "");
                }
            }

            return detailAddress;
        }

        #endregion
    }
}
