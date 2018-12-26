using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Localization;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Vapps.Helpers;

namespace Vapps.DataStatistics
{
    public class UniversalDataStatisticsManager : VappsDomainServiceBase, IUniversalDataStatisticsManager
    {
        public IRepository<UniversalDataStatistics, long> Repository { get; }
        public IQueryable<UniversalDataStatistics> Statisticses => Repository.GetAll().AsNoTracking();

        private readonly IUnitOfWorkManager _unitOfWorkManager;
        private readonly IDateTimeHelper _dateTimeHelper;
        private readonly ILocalizationManager _localizationManager;

        public UniversalDataStatisticsManager(IRepository<UniversalDataStatistics, long> repository,
            IUnitOfWorkManager unitOfWorkManager,
            IDateTimeHelper dateTimeHelper,
            ILocalizationManager localizationManager)
        {
            Repository = repository;
            _unitOfWorkManager = unitOfWorkManager;
            _dateTimeHelper = dateTimeHelper;
            _localizationManager = localizationManager;
        }

        /// <summary>
        /// 基础数据统计
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task AccessDataDailyStatistics(int tenantId, DateTime date)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var dataStatistics = await FindByDateAsync(date, UniversalDataType.AccessData);
                if (dataStatistics != null)
                    return;

                var startTime = date;
                var endTime = date.AddDays(1);


                List<ChannelAccessData> channelDatas = new List<ChannelAccessData>();

                dataStatistics = new UniversalDataStatistics()
                {
                    Date = date,
                    TenantId = tenantId,
                    DataType = UniversalDataType.AccessData,
                    DataStatistics = JsonConvert.SerializeObject(channelDatas),
                };

                await CreateAsync(dataStatistics);
            }
        }

        /// <summary>
        /// 分享数据统计
        /// </summary>
        /// <param name="tenantId"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        [UnitOfWork]
        public async Task ShareDataDailyStatistics(int tenantId, DateTime date)
        {
            using (_unitOfWorkManager.Current.SetTenantId(tenantId))
            {
                var dataStatistics = await FindByDateAsync(date, UniversalDataType.ShareData);
                if (dataStatistics != null)
                    return;

                var startTime = date;
                var endTime = date.AddDays(1);

                List<ShareDataStatistics> shareDataStatistics = new List<ShareDataStatistics>();

                dataStatistics = new UniversalDataStatistics()
                {
                    Date = date,
                    TenantId = tenantId,
                    DataType = UniversalDataType.ShareData,
                    DataStatistics = JsonConvert.SerializeObject(shareDataStatistics),
                };
                await CreateAsync(dataStatistics);
            }
        }

        /// <summary>
        /// 根据日期获取数据概况
        /// </summary>
        /// <param name="date"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public async Task<UniversalDataStatistics> FindByDateAsync(DateTime date, UniversalDataType type)
        {
            return await Statisticses.FirstOrDefaultAsync(uds => uds.Date == date && uds.DataType == type);
        }

        /// <summary>
        /// 根据id获取数据概况
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<UniversalDataStatistics> GetByAsync(long id)
        {
            return await Repository.GetAsync(id);
        }

        /// <summary>
        /// 添加通用数据概况
        /// </summary>
        /// <param name="statistics"></param>
        public async Task CreateAsync(UniversalDataStatistics statistics)
        {
            await Repository.InsertAsync(statistics);
        }

        /// <summary>
        /// 修改通用数据概况
        /// </summary>
        /// <param name="statistics"></param>
        public async Task UpdateAsync(UniversalDataStatistics statistics)
        {
            await Repository.UpdateAsync(statistics);
        }

        /// <summary>
        /// 删除通用数据概况
        /// </summary>
        /// <param name="statistics"></param>
        public async Task DeleteAsync(UniversalDataStatistics statistics)
        {
            await Repository.DeleteAsync(statistics);
        }

        /// <summary>
        /// 删除通用数据概况
        /// </summary>
        /// <param name="id"></param>
        public async Task DeleteAsync(long id)
        {
            var statistics = await Repository.FirstOrDefaultAsync(id);

            if (statistics != null)
                await Repository.DeleteAsync(statistics);
        }
    }
}
