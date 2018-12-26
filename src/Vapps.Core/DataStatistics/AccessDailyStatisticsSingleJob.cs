using Abp.BackgroundJobs;
using Abp.Dependency;
using Abp.Domain.Uow;
using Abp.Threading;

namespace Vapps.DataStatistics
{
    public class AccessDailyStatisticsSingleJob : BackgroundJob<AccessDailyStatisticsSingleJobArgs>, ITransientDependency
    {
        private readonly IUniversalDataStatisticsManager _universalDataStatisticsManager;

        public AccessDailyStatisticsSingleJob(
            IUniversalDataStatisticsManager universalDataStatisticsManager)
        {
            _universalDataStatisticsManager = universalDataStatisticsManager;
        }

        [UnitOfWork]
        public override void Execute(AccessDailyStatisticsSingleJobArgs args)
        {
            AsyncHelper.RunSync(async () =>
            {
                await _universalDataStatisticsManager.AccessDataDailyStatistics(args.TenantId, args.Date);
                await _universalDataStatisticsManager.ShareDataDailyStatistics(args.TenantId, args.Date);
            });
        }
    }
}
