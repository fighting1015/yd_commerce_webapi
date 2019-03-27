using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Castle.Core.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Hangfire;
using Abp.Threading;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel;

namespace Vapps.ECommerce.Shippings.Tracking.Jobs
{
    /// <summary>
    /// 单个租户物流任务
    /// </summary>
    public class SingleShipmentTrackSyncJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IShipmentManager _shipmentManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IShipmentTracker _shipmentTracker;
        private readonly ILogger _logger;

        public SingleShipmentTrackSyncJob(
            IShipmentManager shipmentManager,
            IUnitOfWorkManager unitOfWorkManager,
            IShipmentTracker shipmentTracker,
            ILogger logger)
        {
            this._shipmentManager = shipmentManager;
            this._unitOfWorkManager = unitOfWorkManager;
            this._shipmentTracker = shipmentTracker;
            this._logger = logger;
        }

        [AutomaticRetry(Attempts = 3)]
        [DisplayName("物流跟踪任务, 租户id:{0}")]
        [Queue("shipmenttracker")]
        [UnitOfWork]
        public override void Execute(int arg)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(arg))
                {
                    var status = new List<ShippingStatus> {
                            ShippingStatus.NoTrace,
                            ShippingStatus.Taked,
                            ShippingStatus.OnPassag,
                            ShippingStatus.DestinationCity,
                            ShippingStatus.Delivering,
                            ShippingStatus.Issue
                    };

                    var shipments = await _shipmentManager.Shipments
                         .Include(o => o.Items)
                         .Include(o => o.Order)
                         .Where(s => status.Contains(s.Status)).ToListAsync();

                    foreach (var shipment in shipments)
                    {
                        try
                        {
                            var trace = await _shipmentTracker.GetShipmentTracesAsync(shipment, true);
                        }
                        catch (Exception)
                        {
                            //_logger.Error(string.Format("订单跟踪失败,{0}。", exc.Message), exc);
                        }
                    }
                }
            });
        }
    }
}
