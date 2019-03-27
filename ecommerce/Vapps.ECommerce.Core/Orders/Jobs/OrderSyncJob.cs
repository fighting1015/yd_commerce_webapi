using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Vapps.MultiTenancy;

namespace Vapps.ECommerce.Orders.Jobs
{
    /// <summary>
    /// 订单同步任务
    /// </summary>
    public class OrderSyncJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IBackgroundJobManager _backgroundJobManager;
        private readonly TenantManager _tenantManager;

        public OrderSyncJob(
            IUnitOfWorkManager unitOfWorkManager,
            IBackgroundJobManager backgroundJobManager,
            TenantManager tenantManager)
        {
            this._unitOfWorkManager = unitOfWorkManager;
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
                _backgroundJobManager.Enqueue<OrderSyncSingleJob, int>(tenant.Id);
            }
        }
    }
}
