using System.ComponentModel.DataAnnotations;
using Abp.MultiTenancy;

namespace Vapps.Authorization.Accounts.Dto
{
    public class IsTenantAvailableInput
    {
        /// <summary>
        /// 租户系统名称
        /// </summary>
        [Required]
        [MaxLength(AbpTenantBase.MaxTenancyNameLength)]
        public string TenancyName { get; set; }
    }
}