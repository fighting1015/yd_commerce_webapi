using Abp.BackgroundJobs;
using Abp.Dependency;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vapps.MultiTenancy;

namespace Vapps.Advert.AdvertStatistics.Jobs
{
    /// <summary>
    /// 订单同步任务
    /// </summary>
    public class AdvertDailyStatisticSyncJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly TenantManager _tenantManager;

        public AdvertDailyStatisticSyncJob(
            IBackgroundJobManager backgroundJobManager,
            TenantManager tenantManager)
        {
            this._backgroundJobManager = backgroundJobManager;
            this._tenantManager = tenantManager;
        }

        [Queue("advertdailystatistic")]
        public override void Execute(int arg)
        {
            var tenants = _tenantManager.Tenants.AsNoTracking().ToList();
            foreach (var tenant in tenants)
            {
                _backgroundJobManager.Enqueue<AdvertDailyStatisticSyncSingleJob, int>(tenant.Id);
            }
        }
    }
}
