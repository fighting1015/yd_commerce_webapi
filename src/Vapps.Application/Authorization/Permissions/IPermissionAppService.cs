using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Vapps.Authorization.Permissions.Dto;

namespace Vapps.Authorization.Permissions
{
    public interface IPermissionAppService : IApplicationService
    {
        ListResultDto<FlatPermissionWithLevelDto> GetAllPermissions();
    }
}
