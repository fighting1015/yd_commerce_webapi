using Abp.Domain.Repositories;
using Abp.Extensions;
using Castle.Core.Logging;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.Addresses;
using Vapps.ECommerce.Payments;
using Vapps.ECommerce.Products;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Stores;
using Vapps.States;
using Vapps.States.Cache;

namespace Vapps.ECommerce.Orders
{
    public class OrderManager : VappsDomainServiceBase, IOrderManager
    {
        #region Ctor

        public IRepository<Order, long> OrderRepository { get; }

        public IQueryable<Order> Orderes => OrderRepository.GetAll().AsNoTracking();

        public IRepository<OrderItem, long> OrderItemRepository { get; }

        public IQueryable<OrderItem> OrderItemes => OrderItemRepository.GetAll().AsNoTracking();

        private readonly IStoreManager _storeManager;
        private readonly IProductManager _productManager;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IProductAttributeManager _productAttributeManager;
        private readonly ILogisticsManager _logisticsManager;
        private readonly ILogger _logger;

        private readonly IStateManager _stateManager;
        private readonly IAddressManager _addressManager;
        private readonly IStateCache _stateCache;

        public OrderManager(IRepository<Order, long> OrderRepository,
            IRepository<OrderItem, long> OrderItemRepository,
            IStoreManager storeManager,
            IProductManager productManager,
            IProductAttributeFormatter productAttributeFormatter,
            IProductAttributeManager productAttributeManager,
            ILogisticsManager logisticsManager,
            ILogger logger,
            IStateManager stateManager,
            IAddressManager addressManager,
            IStateCache stateCache)
        {
            this.OrderRepository = OrderRepository;
            this.OrderItemRepository = OrderItemRepository;
            this._storeManager = storeManager;
            this._productManager = productManager;
            this._productAttributeFormatter = productAttributeFormatter;
            this._productAttributeManager = productAttributeManager;
            this._logisticsManager = logisticsManager;
            this._logger = logger;
            this._stateManager = stateManager;
            this._addressManager = addressManager;
            this._stateCache = stateCache;
        }

        #endregion

        #region Order

        /// <summary>
        /// 根据id查找订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Order> FindByIdAsync(long id)
        {
            return await OrderRepository.FirstOrDefaultAsync(id);
        }


        /// <summary>
        /// 根据 订单号 查找订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Order> FindByOrderNumberAsync(string orderNumber)
        {
            return await OrderRepository.FirstOrDefaultAsync(o => o.OrderNumber.Contains(orderNumber));
        }


        /// <summary>
        /// 根据id获取订单
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<Order> GetByIdAsync(long id)
        {
            return await OrderRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加订单
        /// </summary>
        /// <param name="order"></param>
        public virtual async Task CreateAsync(Order order)
        {
            await OrderRepository.InsertAsync(order);
        }

        /// <summary>
        /// 更新订单
        /// </summary>
        /// <param name="order"></param>
        public virtual async Task UpdateAsync(Order order)
        {
            await OrderRepository.UpdateAsync(order);
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="order"></param>
        public virtual async Task DeleteAsync(Order order)
        {
            await OrderRepository.DeleteAsync(order);
        }

        /// <summary>
        /// 删除订单
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteAsync(long id)
        {
            var order = await OrderRepository.FirstOrDefaultAsync(id);

            if (order != null)
                await OrderRepository.DeleteAsync(order);
        }

        /// <summary>
        /// 导入订单
        /// </summary>
        /// <param name="orderImport"></param>
        /// <returns></returns>
        public async Task<bool> ImportOrder(OrderImport orderImport)
        {
            //跳过已取消订单
            if (orderImport.OrderStatus == OrderStatus.Canceled
                && orderImport.ShipName.IsNullOrEmpty())
            {
                return false;
            }

            if (!orderImport.CustomerComment.IsNullOrEmpty())
            {
                if (orderImport.CustomerComment.Trim().ToLower() == "x" ||
                orderImport.CustomerComment.Trim() == "shua")

                    return false;
            }

            if (!orderImport.AdminComment.IsNullOrEmpty())
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

            var order = await FindByOrderNumberAsync(orderImport.OrderNumber);

            if (order != null)
            {
                if (order.Shipments.Count == 0 || order.ShippingStatus == ShippingStatus.NotYetShipped)
                {
                    await AddShipment(orderImport, order);
                    await UpdateAsync(order);
                }
                else
                {
                    order.Shipments.FirstOrDefault().LogisticsNumber = orderImport.TrackingNumber;
                }

                order.RewardAmount = orderImport.Reward;
                order.CreationTime = orderImport.CreatedOnUtc;

                await UpdateAsync(order);

                return true;
            }

            var address = await PrepareShippingAddress(orderImport);
            order = new Order()
            {
                OrderSource = orderImport.OrderSource,
                StoreId = orderImport.StoreId > 0 ? orderImport.StoreId : _storeManager.Stores.FirstOrDefaultAsync()?.Id ?? 0,
                OrderNumber = orderImport.OrderNumber,
                UserId = 0, // TODO:创建用户
                TotalAmount = orderImport.OrderTotal,
                DiscountAmount = orderImport.DiscountAmount,
                OrderStatus = orderImport.OrderStatus,
                PaymentStatus = PaymentStatus.Pending,
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
            };

            //插入新订单
            await CreateAsync(order);

            foreach (var orderItemImport in orderItemImportList)
            {
                //save item
                var orderItem = new OrderItem()
                {
                    ProductId = orderItemImport.Product.Id,
                    Price = orderItemImport.Product.Price,
                    UnitPrice = orderItemImport.Product.Price,
                    OriginalProductCost = orderItemImport.Combin.OverriddenGoodCost.HasValue ? orderItemImport.Combin.OverriddenGoodCost.Value : orderItemImport.Product.GoodCost,
                    Quantity = orderItemImport.Quantity * orderImport.PackageNum,
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
            await UpdateAsync(order);

            return true;
        }

        /// <summary>
        /// 获取订单产品信息
        /// </summary>
        /// <param name="productSku"></param>
        /// <param name="orderNumber"></param>
        /// <returns></returns>
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
            if (orderImport.ShipName.IsNullOrEmpty())
                return null;

            if (orderImport.TrackingNumber.IsNullOrEmpty())
                return null;

            order.ShippingStatus = ShippingStatus.Taked;
            var trackingNumber = orderImport.TrackingNumber.Replace("'", "");

            var logistics = await _logisticsManager.FindByNameAsync(orderImport.ShipName);
            if (logistics == null)
                return null;

            Shipment shipment;
            if (order.Shipments.Any())
            {
                shipment = order.Shipments.FirstOrDefault();
                shipment.LogisticsId = logistics.Id;
                //shipment.TrackingNumber = trackingNumber;
                shipment.CreationTime = orderImport.CreatedOnUtc;
            }
            else
            {
                shipment = new Shipment()
                {
                    LogisticsId = logistics.Id,
                    OrderId = order.Id,
                    CreationTime = orderImport.CreatedOnUtc
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
                foreach (var item in order.Items)
                {
                    shipment.Items.Add(new ShipmentItem()
                    {
                        OrderItemId = item.Id,
                        Quantity = item.Quantity,
                    });
                }

                order.Shipments.Add(shipment);
            }

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
            var citys = await _stateManager.Cities.Where(c => c.ProvinceId == province.Id).ToListAsync();
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
            if (!destrictName.IsNullOrEmpty())
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

        #region OrderItem

        /// <summary>
        /// 根据订单id查找订单条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<List<OrderItem>> FindOrderItemByOrderIdAsync(long orderId)
        {
            return await OrderItemRepository.GetAll().Where(tl => tl.OrderId == orderId).ToListAsync();
        }

        /// <summary>
        /// 根据id查找订单条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<OrderItem> FindOrderItemByIdAsync(long id)
        {
            return await OrderItemRepository.FirstOrDefaultAsync(id);
        }

        /// <summary>
        /// 根据id获取订单条目
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual async Task<OrderItem> GetOrderItemByIdAsync(long id)
        {
            return await OrderItemRepository.GetAsync(id);
        }

        /// <summary>
        /// 添加订单条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task CreateOrderItemAsync(OrderItem item)
        {
            await OrderItemRepository.InsertAsync(item);
        }

        /// <summary>
        /// 更新订单条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task UpdateOrderItemAsync(OrderItem item)
        {
            await OrderItemRepository.UpdateAsync(item);
        }

        /// <summary>
        /// 删除订单条目
        /// </summary>
        /// <param name="item"></param>
        public virtual async Task DeleteOrderItemAsync(OrderItem item)
        {
            await OrderItemRepository.DeleteAsync(item);
        }

        /// <summary>
        /// 删除订单条目
        /// </summary>
        /// <param name="id"></param>
        public virtual async Task DeleteOrderItemAsync(long id)
        {
            var order = await OrderItemRepository.FirstOrDefaultAsync(id);

            if (order != null)
                await OrderItemRepository.DeleteAsync(order);
        }

        #endregion
    }
}
