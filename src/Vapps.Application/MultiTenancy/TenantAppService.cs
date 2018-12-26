using Abp;
using Abp.Application.Features;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Authorization.Users;
using Abp.Domain.Uow;
using Abp.Linq.Extensions;
using Abp.Runtime.Security;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Vapps.Authorization;
using Vapps.Authorization.Users;
using Vapps.Editions.Dto;
using Vapps.MultiTenancy.Dto;
using Vapps.Url;

namespace Vapps.MultiTenancy
{
    [AbpAuthorize(AdminPermissions.UserManage.Tenants.Self)]
    public class TenantAppService : VappsAppServiceBase, ITenantAppService
    {
        private readonly IAppUrlService _appUrlService;

        public TenantAppService(IAppUrlService appUrlService)
        {
            _appUrlService = appUrlService;
        }

        #region Tenant Mange Methods

        /// <summary>
        /// 获取所有租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<TenantListDto>> GetTenants([FromQuery]GetTenantsInput input)
        {
            var query = TenantManager.Tenants
                .Include(t => t.Edition)
                .WhereIf(!input.TenancyName.IsNullOrEmpty(), t => t.TenancyName.Contains(input.TenancyName))
                .WhereIf(!input.Name.IsNullOrEmpty(), t => t.Name.Contains(input.Name))
                .WhereIf(input.IsActive.HasValue, t => t.IsActive == input.IsActive.Value)
                .WhereIf(input.CreationDateStart.HasValue, t => t.CreationTime >= input.CreationDateStart.Value)
                .WhereIf(input.CreationDateEnd.HasValue, t => t.CreationTime <= input.CreationDateEnd.Value)
                .WhereIf(input.SubscriptionEndDateStart.HasValue, t => t.SubscriptionEndDateUtc >= input.SubscriptionEndDateStart.Value.ToUniversalTime())
                .WhereIf(input.SubscriptionEndDateEnd.HasValue, t => t.SubscriptionEndDateUtc <= input.SubscriptionEndDateEnd.Value.ToUniversalTime())
                .WhereIf(input.EditionIdSpecified, t => t.EditionId == input.EditionId);

            var tenantCount = await query.CountAsync();
            var tenants = await query.OrderBy(input.Sorting).PageBy(input).ToListAsync();

            return new PagedResultDto<TenantListDto>(
               tenantCount,
               ObjectMapper.Map<List<TenantListDto>>(tenants)
               );
        }

        /// <summary>
        /// 创建租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.UserManage.Tenants.Create)]
        [UnitOfWork(IsDisabled = true)]
        public async Task CreateTenant(CreateTenantInput input)
        {
            await TenantManager.CreateWithAdminUserAsync(input.TenancyName,
                input.Name,
                input.AdminPassword,
                input.AdminEmailAddress,
                input.PhoneNumber,
                input.ConnectionString,
                input.IsActive,
                input.EditionId,
                input.ShouldChangePasswordOnNextLogin,
                input.SendActivationEmail,
                input.SubscriptionEndDateUtc?.ToUniversalTime(),
                input.IsInTrialPeriod,
                _appUrlService.CreateEmailActivationUrlFormat(input.TenancyName)
            );
        }

        /// <summary>
        /// 获取租户详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.UserManage.Tenants.Edit)]
        public async Task<GetTenantForEditOutput> GetTenantForEdit([FromQuery]EntityDto input)
        {
            var tenantEditDto = ObjectMapper.Map<TenantEditDto>(await TenantManager.GetByIdAsync(input.Id));

            tenantEditDto.ConnectionString = SimpleStringCipher.Instance.Decrypt(tenantEditDto.ConnectionString);

            return new GetTenantForEditOutput()
            {
                Tenant = tenantEditDto,
                Features = await GetTenantFeaturesForEdit(new EntityDto() { Id = input.Id }),//获取租户特性
            };
        }

        /// <summary>
        /// 更新租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.UserManage.Tenants.Edit)]
        public async Task UpdateTenant(TenantEditDto input)
        {
            input.ConnectionString = SimpleStringCipher.Instance.Encrypt(input.ConnectionString);
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            ObjectMapper.Map(input, tenant);
            await TenantManager.UpdateAsync(tenant);

            if (input.Features != null)
            {
                //更新租户特性
                await TenantManager.SetFeatureValuesAsync(input.Id, input.Features.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
            }
        }

        /// <summary>
        /// 删除租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.UserManage.Tenants.Delete)]
        public async Task DeleteTenant(EntityDto input)
        {
            var tenant = await TenantManager.GetByIdAsync(input.Id);
            await TenantManager.DeleteAsync(tenant);
        }

        /// <summary>
        /// 获取租户特性详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.UserManage.Tenants.ChangeFeatures)]
        public async Task<GetTenantFeaturesEditOutput> GetTenantFeaturesForEdit([FromQuery]EntityDto input)
        {
            var features = FeatureManager.GetAll()
                .Where(f => f.Scope.HasFlag(FeatureScopes.Tenant));
            var featureValues = await TenantManager.GetFeatureValuesAsync(input.Id);

            return new GetTenantFeaturesEditOutput
            {
                Features = ObjectMapper.Map<List<FlatFeatureDto>>(features).OrderBy(f => f.DisplayName).ToList(),
                FeatureValues = featureValues.Select(fv => new NameValueDto(fv)).ToList()
            };
        }

        /// <summary>
        /// 更新租户特性
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.UserManage.Tenants.ChangeFeatures)]
        public async Task UpdateTenantFeatures(UpdateTenantFeaturesInput input)
        {
            await TenantManager.SetFeatureValuesAsync(input.Id, input.FeatureValues.Select(fv => new NameValue(fv.Name, fv.Value)).ToArray());
        }

        /// <summary>
        /// 重置租户详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [AbpAuthorize(AdminPermissions.UserManage.Tenants.ChangeFeatures)]
        public async Task ResetTenantSpecificFeatures(EntityDto input)
        {
            await TenantManager.ResetAllFeaturesAsync(input.Id);
        }

        /// <summary>
        /// 解锁租户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task UnlockTenantAdmin(EntityDto input)
        {
            using (CurrentUnitOfWork.SetTenantId(input.Id))
            {
                var tenantAdmin = await UserManager.FindByNameAsync(AbpUserBase.AdminUserName);
                if (tenantAdmin != null)
                {
                    tenantAdmin.Unlock();
                }
            }
        }

        #endregion
    }
}