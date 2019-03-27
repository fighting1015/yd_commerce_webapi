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
using Vapps.Advert.AdvertAccounts.Sync;
using Vapps.ECommerce.Orders;

namespace Vapps.Advert.AdvertAccounts.Jobs
{
    /// <summary>
    /// 单个租户订单同步任务
    /// </summary>
    public class AdvertOrderSyncSingleJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAdvertAccountManager _advertAccountManager;
        private readonly IAdvertAccountSyncor _tenantAdvertAccountSyncor;
        private readonly ILogger _logger;
        private readonly IOrderImportor _orderImportor;

        public AdvertOrderSyncSingleJob(
            IIocResolver iocResolver,
            IUnitOfWorkManager unitOfWorkManager,
            IAdvertAccountManager advertAccountManager,
            ILogger logger,
            IOrderImportor orderImportor)
        {
            this._unitOfWorkManager = unitOfWorkManager;
            this._advertAccountManager = advertAccountManager;
            this._tenantAdvertAccountSyncor = iocResolver.Resolve<TenantAdvertAccountSyncor>();
            this._logger = logger;
            this._orderImportor = orderImportor;
        }

        [AutomaticRetry(Attempts = 3)]
        [DisplayName("广告订单同步任务, 租户id:{0}")]
        [Queue("order")]
        [UnitOfWork]
        public override void Execute(int arg)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(arg))
                {
                    try
                    {
                        var accounts = await _advertAccountManager.AdvertAccounts.Where(a => a.Channel == AdvertChannel.Tencent).ToListAsync();
                        var endDate = DateTime.Now.GetStartTimeOfDate().AddDays(1);
                        var startDate = endDate.AddDays(-2);

                        foreach (var account in accounts)
                        {
                            if (!account.IsAuth())
                                continue;

                            if (account.IsAuthExpires())
                                await _tenantAdvertAccountSyncor.RefreshAccessTokenAsync(account);

                            await _tenantAdvertAccountSyncor.SyncOrders(account, startDate, endDate);
                        }
                    }
                    catch (Exception exc)
                    {
                        _logger.Error(string.Format("订单同步失败。{0}", exc.Message), exc);
                    }
                }
            });
        }
    }
}
