using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using Vapps.MultiTenancy;

namespace Vapps.Authorization.Accounts.Dto
{
    [AutoMapFrom(typeof(Tenant))]
    public class CurrentTenantInfoDto : EntityDto
    {
        /// <summary>
        /// 租户系统名称
        /// </summary>
        public string TenancyName { get; set; }

        /// <summary>
        /// 租户显示名称
        /// </summary>
        public string Name { get; set; }
    }
}