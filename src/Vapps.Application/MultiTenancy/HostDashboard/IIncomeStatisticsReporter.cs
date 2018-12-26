using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Vapps.MultiTenancy.HostDashboard.Dto;

namespace Vapps.MultiTenancy.HostDashboard
{
    public interface IIncomeStatisticsService
    {
        Task<List<IncomeStastistic>> GetIncomeStatisticsData(DateTime startDate, DateTime endDate,
            ChartDateInterval dateInterval);
    }
}