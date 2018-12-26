using System.Threading.Tasks;
using Abp.Application.Services;
using Vapps.Sessions.Dto;

namespace Vapps.Sessions
{
    public interface ISessionAppService : IApplicationService
    {
        Task<GetCurrentLoginInformationsOutput> GetCurrentLoginInformations();
    }
}
