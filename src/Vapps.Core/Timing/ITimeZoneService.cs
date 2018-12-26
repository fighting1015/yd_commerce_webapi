using System.Threading.Tasks;
using Abp.Configuration;
using System.Collections.Generic;
using Abp.Application.Services.Dto;
using System;

namespace Vapps.Timing
{
    public interface ITimeZoneService
    {
        Task<string> GetDefaultTimezoneAsync(SettingScopes scope, int? tenantId);

        TimeZoneInfo FindTimeZoneById(string timezoneId);

        List<NameValueDto> GetWindowsTimezones();
    }
}
