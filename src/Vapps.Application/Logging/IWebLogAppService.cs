using Abp.Application.Services;
using Vapps.Dto;
using Vapps.Logging.Dto;

namespace Vapps.Logging
{
    public interface IWebLogAppService : IApplicationService
    {
        /// <summary>
        /// 获取最近日志
        /// </summary>
        /// <returns></returns>
        GetLatestWebLogsOutput GetLatestWebLogs();

        /// <summary>
        /// 下载日志文件
        /// </summary>
        /// <returns></returns>
        FileDto DownloadWebLogs();
    }
}
