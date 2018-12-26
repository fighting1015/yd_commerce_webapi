using System.Threading.Tasks;
using Abp.Application.Services;
using Vapps.Install.Dto;

namespace Vapps.Install
{
    public interface IInstallAppService : IApplicationService
    {
        Task Setup(InstallDto input);

        AppSettingsJsonDto GetAppSettingsJson();

        CheckDatabaseOutput CheckDatabase();
    }
}