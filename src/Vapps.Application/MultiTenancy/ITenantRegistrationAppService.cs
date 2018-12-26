using System.Threading.Tasks;
using Abp.Application.Services;
using Vapps.MultiTenancy.Dto;
using Vapps.Editions.Dto;

namespace Vapps.MultiTenancy
{
    public interface ITenantRegistrationAppService : IApplicationService
    {
        Task<RegisterTenantOutput> RegisterTenant(RegisterTenantInput input);

    }
}