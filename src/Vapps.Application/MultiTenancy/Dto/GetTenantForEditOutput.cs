using Abp.Application.Services.Dto;
using Abp.AutoMapper;
using System.ComponentModel.DataAnnotations;

namespace Vapps.MultiTenancy.Dto
{
    public class GetTenantForEditOutput : EntityDto
    {
        /// <summary>
        /// 租户详情
        /// </summary>
        public TenantEditDto Tenant { get; set; }

        /// <summary>
        /// 特性(限制)
        /// </summary>
        public GetTenantFeaturesEditOutput Features { get; set; }
    }
}
