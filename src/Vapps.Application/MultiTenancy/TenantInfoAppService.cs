using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.UI;
using System.Threading.Tasks;
using Vapps.Authorization.Roles;
using Vapps.Authorization.Users;
using Vapps.Media;
using Vapps.MultiTenancy.Dto;

namespace Vapps.MultiTenancy
{
    /// <summary>
    /// 租户基本信息
    /// </summary>
    [AbpAuthorize]
    public class TenantInfoAppService : VappsAppServiceBase, ITenantInfoAppService
    {
        private readonly UserManager _userManager;
        private readonly RoleManager _roleManager;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly IPictureManager _pictureManager;

        public TenantInfoAppService(UserManager userManager,
            RoleManager roleManager,
            IUnitOfWorkManager unitOfWorkManager,
            IPictureManager pictureManager)
        {
            this._userManager = userManager;
            this._roleManager = roleManager;
            this._unitOfWorkManager = unitOfWorkManager;
            this._pictureManager = pictureManager;
        }

        /// <summary>
        /// 获取租户基本信息
        /// </summary>
        /// <returns></returns>
        public async Task<TenantInfoEditDto> GetTenantInfoForEdit()
        {
            if (!AbpSession.TenantId.HasValue)
                throw new UserFriendlyException(L("CanNotFindTenant"));

            var tenant = await TenantManager.GetByIdAsync(AbpSession.TenantId.Value);
            var tenantDto = ObjectMapper.Map<TenantInfoEditDto>(tenant);

            tenantDto.LogoUrl = await _pictureManager.GetPictureUrlAsync(tenant.LogoId);
            tenantDto.BackgroundPictureUrl = await _pictureManager.GetPictureUrlAsync(tenant.BackgroundPictureId);

            return tenantDto;
        }

        /// <summary>
        /// 更新租户资料
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UpdateTenantInfo(TenantInfoEditDto input)
        {
            if (!AbpSession.TenantId.HasValue)
                throw new UserFriendlyException(L("CanNotFindTenant"));

            var tenant = await TenantManager.GetByIdAsync(AbpSession.TenantId.Value);
            if (input.LogoId != tenant.LogoId)
            {
                await _pictureManager.DeleteAsync(tenant.LogoId);
                tenant.LogoId = input.LogoId;
            }

            if (input.BackgroundPictureId != tenant.BackgroundPictureId)
            {
                await _pictureManager.DeleteAsync(tenant.BackgroundPictureId);
                tenant.BackgroundPictureId = input.BackgroundPictureId;
            }

            if (input.TenancyName != tenant.Name)
            {
                var user = await _userManager.UserStore.FindMainUser4PlatformByTenantIdAsync(tenant.Id);
                user.UserName = input.TenancyName;
                user.NormalizedUserName = input.TenancyName.ToLower();
                await _userManager.UpdateAsync(user);
                await TenantManager.ChangeTenantNameAsync(tenant, input.TenancyName);
            }

            tenant.Description = input.Description;
            tenant.Tagline = input.Tagline;
            await TenantManager.UpdateAsync(tenant);
        }
    }
}
