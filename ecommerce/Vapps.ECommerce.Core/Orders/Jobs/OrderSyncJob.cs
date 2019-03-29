using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using Hangfire;
using Hangfire.States;
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
        private readonly OrderSyncSingleJob _orderSyncSingleJob;


        public OrderSyncJob(
            IUnitOfWorkManager unitOfWorkManager,
            IBackgroundJobManager backgroundJobManager,
            TenantManager tenantManager,
            OrderSyncSingleJob orderSyncSingleJob)
        {
            this._unitOfWorkManager = unitOfWorkManager;
            this._backgroundJobManager = backgroundJobManager;
            this._tenantManager = tenantManager;
            this._orderSyncSingleJob = orderSyncSingleJob;
        }

        [Queue("order")]
        [UnitOfWork]
        public override void Execute(int arg)
        {
            AsyncHelper.RunSync(async () =>
            {
                var tenants = await _tenantManager.Tenants.AsNoTracking().ToListAsync();

                //client.Create(() => Test(), state);

                var client = new BackgroundJobClient();
                var state = new EnqueuedState("tenantorder");

                foreach (var tenant in tenants)
                {
                    client.Create<OrderSyncSingleJob>(job => job.Execute(tenant.Id), state);
                    //BackgroundJob.Enqueue(job => job.Execute(tenant.Id));

                    //await _backgroundJobManager.EnqueueAsync<OrderSyncSingleJob, int>(tenant.Id);
                }
            });
        }

        public void Test()
        {
            return;
        }
    }
}
