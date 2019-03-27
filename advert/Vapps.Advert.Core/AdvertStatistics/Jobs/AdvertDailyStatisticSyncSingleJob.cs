using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;
using Castle.Core.Logging;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel;
using Vapps.Advert.AdvertAccounts;
using Vapps.Advert.AdvertAccounts.Sync;

namespace Vapps.Advert.AdvertStatistics.Jobs
{
    /// <summary>
    /// 单个租户订单同步任务
    /// </summary>
    public class AdvertDailyStatisticSyncSingleJob : BackgroundJob<int>, ITransientDependency
    {
        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IAdvertAccountManager _advertAccountManager;
        private readonly IAdvertAccountSyncor _tenantAdvertAccountSyncor;
        private readonly IAdvertAccountSyncor _toutiaoAdvertAccountSyncor;
        private readonly ILogger _logger;

        public AdvertDailyStatisticSyncSingleJob(
            IIocResolver iocResolver,
            IUnitOfWorkManager unitOfWorkManager,
            IAdvertAccountManager advertAccountManager,
            ILogger logger)
        {
            this._unitOfWorkManager = unitOfWorkManager;
            this._advertAccountManager = advertAccountManager;
            this._tenantAdvertAccountSyncor = iocResolver.Resolve<TenantAdvertAccountSyncor>();
            this._toutiaoAdvertAccountSyncor = iocResolver.Resolve<ToutiaoAdvertAccountSyncor>();
            this._logger = logger;
        }

        [AutomaticRetry(Attempts = 3)]
        [DisplayName("广告消耗同步任务, 租户id:{0}")]
        [Queue("advertdailystatistic")]
        [UnitOfWork]
        public override void Execute(int arg)
        {
            AsyncHelper.RunSync(async () =>
            {
                using (_unitOfWorkManager.Current.SetTenantId(arg))
                {
                    var accounts = await _advertAccountManager.AdvertAccounts.ToListAsync();
                    var startDate = DateTime.Now.GetStartTimeOfDate().AddDays(-1);
                    var endDate = DateTime.Now.GetEndTimeOfDate();

                    foreach (var account in accounts)
                    {
                        if (!account.IsAuth())
                            continue;
                        try
                        {

                            if (account.Channel == AdvertChannel.Tencent)
                            {
                                await _tenantAdvertAccountSyncor.QueryAccountFundsAsync(account);
                                await _tenantAdvertAccountSyncor.QueryDailyReportsAsync(account, startDate, endDate);
                            }
                            else
                            {

                                await _toutiaoAdvertAccountSyncor.QueryAccountFundsAsync(account);
                                await _toutiaoAdvertAccountSyncor.QueryDailyReportsAsync(account, startDate, endDate);
                            }

                        }
                        catch (Exception exc)
                        {
                            _logger.Error(string.Format($"广告账户 {account.DisplayName} 数据同步失败。{exc.Message}"));
                        }
                    }
                }
            });
        }
    }
}
