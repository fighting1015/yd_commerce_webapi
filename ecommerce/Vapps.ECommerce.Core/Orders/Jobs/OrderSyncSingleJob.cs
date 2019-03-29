using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using Castle.Core.Logging;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using System.Linq;
using Vapps.ECommerce.Orders.Toutiao;
using Vapps.ECommerce.Orders.Toutiao.Requests;
using Vapps.ECommerce.Shippings;
using Vapps.ECommerce.Stores;
using Vapps.Filter;

namespace Vapps.ECommerce.Orders.Jobs
{
    /// <summary>
    /// 单个租户订单同步任务
    /// </summary>
    public class OrderSyncSingleJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IStoreManager _storeManager;
        private readonly ILogger _logger;
        private readonly IOrderImportor _orderImportor;

        public OrderSyncSingleJob(
            IUnitOfWorkManager unitOfWorkManager,
            IStoreManager storeManager,
            ILogger logger,
            IOrderImportor orderImportor)
        {
            this._unitOfWorkManager = unitOfWorkManager;
            this._storeManager = storeManager;
            this._logger = logger;
            this._orderImportor = orderImportor;
        }

        [Queue("tenantorder")]
        //[UseQueueFromParameter("tenantorder")]
        [AutomaticRetry(Attempts = 3)]
        [DisplayName("订单同步任务, 租户id:{0}")]
        [UnitOfWork]
        public override void Execute(int arg)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(arg))
                {
                    var stores = await _storeManager.Stores.ToListAsync();

                    foreach (var store in stores)
                    {
                        try
                        {
                            if (store.OrderSource != OrderSource.FxgPd && store.OrderSource != OrderSource.FxgAd)
                                continue;

                            ToutiaoApiHelper.AppKey = store.AppKey;
                            ToutiaoApiHelper.AppSecret = store.AppSecret;

                            bool readEnd = false;
                            int pageIndex = 0;
                            int pageSize = 100;

                            while (!readEnd)
                            {
                                OrderListRequestPara para = new OrderListRequestPara()
                                {
                                    StartTime = DateTime.Now.GetStartTimeOfDate().AddDays(-30).DateTimeString("yyyy/MM/dd HH:mm:ss"),
                                    EndTime = DateTime.Now.GetEndTimeOfDate().DateTimeString("yyyy/MM/dd HH:mm:ss"),
                                    Page = pageIndex.ToString(),
                                    Size = pageSize.ToString(),
                                    OrderBy = "create_time"
                                };

                                var response = await ToutiaoApiHelper.GetOrderList(para);

                                if (response.ErrorNo > 0 || response.Data.Items == null)
                                {
                                    _logger.Error(string.Format("订单同步失败。{0}", response.Message));
                                    break;
                                }

                                foreach (var orderData in response.Data.Items)
                                {
                                    //跳过待确认
                                    if (orderData.OrderStatus == (int)ToutiaoOrderStatus.WaitForComfirm)
                                    {
                                        continue;
                                    }

                                    //跳过已取消订单
                                    if (orderData.OrderStatus == (int)ToutiaoOrderStatus.Canceled && orderData.LogisticsCode.IsNullOrEmpty())
                                    {
                                        continue;
                                    }

                                    if (orderData.BuyerWords.Trim().ToLower() == "x" ||
                                        orderData.SellerWords.Trim().ToLower() == "shua")
                                    {
                                        continue;
                                    }

                                    foreach (var orderItemData in orderData.Childs)
                                    {
                                        if (orderItemData.Code.StartsWith("FXG011"))
                                            continue;

                                        var orderTotal = Convert.ToDecimal(orderItemData.TotalAmount) / 100;
                                        var shipInfo = ToutiaoShipInfoData.GetToutiaoShipInfoById(orderItemData.LogisticsId);

                                        var orderDto = new OrderImport()
                                        {
                                            OrderSource = store.OrderSource,
                                            StoreId = store.Id,
                                            OrderNumber = orderItemData.OrderId,
                                            ProductSku = orderItemData.Code,
                                            OrderStatus = orderItemData.OrderStatus.GetOrderStatus(),
                                            ShippingStatus = orderItemData.OrderStatus.GetShippingStatus(),
                                            OrderTotal = orderTotal,
                                            Reward = orderItemData.GetOrderReward(orderTotal),
                                            ShipTotal = Convert.ToDecimal(orderItemData.PostAmount) / 100,
                                            DiscountAmount = orderItemData.ShopCouponAmount / 100 + orderItemData.CouponAmount / 100,
                                            PackageNum = orderItemData.ComboNum,
                                            PackageName = orderItemData.ProductName,
                                            LogisticsName = shipInfo?.Name,
                                            TrackingNumber = orderItemData.LogisticsCode,
                                            TrackingOrderNumber = orderItemData.LogisticsCode,

                                            CustomerComment = orderItemData.BuyerWords,
                                            AdminComment = orderItemData.SellerWords,

                                            ReceiverName = orderItemData.PostReceiver,
                                            Telephone = orderItemData.PostTel,
                                            FullAddress = orderItemData.PostAddress.Detail,
                                            Province = orderItemData.PostAddress.Provice.Name,
                                            City = orderItemData.PostAddress.City.Name,
                                            District = orderItemData.PostAddress.Town.Name,
                                            Address = orderItemData.PostAddress.Detail,

                                            PlaceOnUtc = orderItemData.LogisticsTime.GetTime(false),
                                            DeliveriedOnUtc = orderItemData.ReceiptTime.GetTime(false),
                                            CreatedOnUtc = orderItemData.CreateTime.GetTime(false),
                                        };

                                        if (orderItemData.OrderStatus == ToutiaoOrderStatus.Canceled || orderItemData.OrderStatus > ToutiaoOrderStatus.Refunse)
                                        {
                                            orderDto.OrderStatus = OrderStatus.Canceled;
                                            orderDto.ShippingStatus = ShippingStatus.IssueWithReject;
                                        }

                                        await _orderImportor.ImportOrderAsync(orderDto);
                                    }
                                }

                                pageIndex += 1;
                                readEnd = (pageIndex) * pageSize > response.Data.Total;
                            }
                        }
                        catch (Exception ex)
                        {
                            _logger.Error(string.Format("订单同步失败。{0}", ex.Message), ex);
                        }
                    }
                }
            });
        }
    }
}
