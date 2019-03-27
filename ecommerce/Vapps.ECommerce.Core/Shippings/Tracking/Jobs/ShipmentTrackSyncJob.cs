using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vapps.MultiTenancy;
using Hangfire;
using Microsoft.EntityFrameworkCore;

namespace Vapps.ECommerce.Shippings.Tracking.Jobs
{
    /// <summary>
    /// 物流跟踪任务
    /// </summary>
    public class ShipmentTrackSyncJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly TenantManager _tenantManager;

        public ShipmentTrackSyncJob(
            IUnitOfWorkManager unitOfWorkManager,
            IBackgroundJobManager backgroundJobManager,
            TenantManager tenantManager)
        {
            this._unitOfWorkManager = unitOfWorkManager;
            this._backgroundJobManager = backgroundJobManager;
            this._tenantManager = tenantManager;
        }

        [Queue("shipmenttracker")]
        [UnitOfWork]
        public override void Execute(int arg)
        {
            var tenants = _tenantManager.Tenants.AsNoTracking().ToList();
            foreach (var tenant in tenants)
            {
                _backgroundJobManager.Enqueue<SingleShipmentTrackSyncJob, int>(tenant.Id);
            }
        }
    }
}
