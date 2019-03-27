using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Abp.Threading;
using Vapps.ECommerce.Shippings.Tracking;
using Vapps.ECommerce.Orders;
using Abp.Domain.Repositories;
using System.ComponentModel;
using Vapps.ECommerce.Stores;
using Vapps.ECommerce.Orders.Toutiao;

namespace Vapps.ECommerce.Shippings.Jobs
{
    /// <summary>
    /// 处理发货
    /// </summary>
    public class ProcessShipJob : BackgroundJob<ProcessShipJobArg>, ITransientDependency
    {
        private readonly IStoreManager _storeManager;
        private readonly IShipmentManager _shipmentManager;
        private readonly IShipmentTracker _shipmentTracker;
        private readonly IOrderProcessingManager _orderProcessingManager;

        public ProcessShipJob(
            IStoreManager storeManager,
            IShipmentManager shipmentManager,
            IShipmentTracker shipmentTracker,
            IOrderProcessingManager orderProcessingManager)
        {
            this._storeManager = storeManager;
            this._shipmentManager = shipmentManager;
            this._shipmentTracker = shipmentTracker;
            this._orderProcessingManager = orderProcessingManager;
        }

        [AutomaticRetry(Attempts = 3)]
        [DisplayName("处理发货事件, 租户id:{0}")]
        [Queue("shipment")]
        [UnitOfWork]
        public override void Execute(ProcessShipJobArg arg)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (UnitOfWorkManager.Current.SetTenantId(arg.TenantId))
                {
                    var shipment = await _shipmentManager.GetByIdAsync(arg.ShipmentId);
                    await _shipmentManager.ShipmentRepository.EnsurePropertyLoadedAsync(shipment, s => s.Order);

                    // 修改订单状态
                    await _orderProcessingManager.CheckOrderStatus(shipment.Order);
                    await _shipmentManager.UpdateAsync(shipment);

                    // 添加物流订阅
                    await _shipmentTracker.OrderTracesSubscribeAsync(shipment);

                    // 回传头条
                    if (shipment.Order.OrderSource == OrderSource.FxgAd ||
                    shipment.Order.OrderSource == OrderSource.FxgPd)
                    {
                        //回传头条
                        var store = await _storeManager.GetByIdAsync(shipment.Order.StoreId);
                        ToutiaoApiHelper.AppKey = store.AppKey;
                        ToutiaoApiHelper.AppSecret = store.AppSecret;
                        //确认订单
                        await ToutiaoApiHelper.OrderStockUpAsync(shipment);
                        //执行发货
                        await ToutiaoApiHelper.AddLogisticsAsync(shipment);
                    }
                    else if (shipment.Order.OrderSource == OrderSource.Tenant)
                    {
                        //回传广点通
                    }
                }
            });
        }
    }
}
