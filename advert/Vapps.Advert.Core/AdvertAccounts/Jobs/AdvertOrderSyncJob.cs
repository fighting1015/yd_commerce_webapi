using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vapps.MultiTenancy;

namespace Vapps.Advert.AdvertAccounts.Jobs
{
    /// <summary>
    /// 订单同步任务
    /// </summary>
    public class AdvertOrderSyncJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly TenantManager _tenantManager;

        public AdvertOrderSyncJob(
            IBackgroundJobManager backgroundJobManager,
            TenantManager tenantManager)
        {
            this._backgroundJobManager = backgroundJobManager;
            this._tenantManager = tenantManager;
        }

        [Queue("order")]
        [UnitOfWork]
        public override void Execute(int arg)
        {
            var tenants = _tenantManager.Tenants.AsNoTracking().ToList();
            foreach (var tenant in tenants)
            {
                _backgroundJobManager.Enqueue<AdvertOrderSyncSingleJob, int>(tenant.Id);
            }
        }
    }
}
