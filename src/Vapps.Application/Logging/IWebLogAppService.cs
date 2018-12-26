using Abp.Application.Services;
using Vapps.Dto;
using Vapps.Logging.Dto;

namespace Vapps.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        GetLatestWebLogsOutput GetLatestWebLogs();

        FileDto DownloadWebLogs();
    }
}
