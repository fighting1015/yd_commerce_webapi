using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Threading;
using Hangfire;
using System.ComponentModel;
using Vapps.Advert.AdvertAccounts.Sync;
using Vapps.ECommerce.Orders;
using Vapps.ECommerce.Shippings;

namespace Vapps.Advert.AdvertAccounts.Jobs
{
    /// <summary>
    /// 处理发货
    /// </summary>
    public class ProcessAdvertShipJob : BackgroundJob<ProcessAdvertShipJobArgs>, ITransientDependency
    {
        private readonly IAdvertAccountSyncor _advertAccountSyncor;
        private readonly IShipmentManager _shipmentManager;

        public ProcessAdvertShipJob(
            IIocResolver iocResolver,
            IShipmentManager shipmentManager)
        {
            this._advertAccountSyncor = iocResolver.Resolve<TenantAdvertAccountSyncor>();
            this._shipmentManager = shipmentManager;
        }

        [AutomaticRetry(Attempts = 3)]
        [DisplayName("处理广告发货事件, 租户id:{0}")]
        [Queue("shipment")]
        [UnitOfWork]
        public override void Execute(ProcessAdvertShipJobArgs arg)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (UnitOfWorkManager.Current.SetTenantId(arg.TenantId))
                {
                    var shipment = await _shipmentManager.GetByIdAsync(arg.ShipmentId);
                    await _shipmentManager.ShipmentRepository.EnsurePropertyLoadedAsync(shipment, s => s.Order);

                    //回传广点通
                    if (shipment.Order.OrderSource != OrderSource.Tenant)
                    {
                        return;
                    }

                    await _advertAccountSyncor.UploadOrderTrackingAsync(shipment);
                }
            });
        }
    }
}
