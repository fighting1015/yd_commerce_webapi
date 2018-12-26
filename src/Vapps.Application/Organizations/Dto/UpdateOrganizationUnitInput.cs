using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace Vapps.Organizations.Dto
{
    public class UpdateOrganizationUnitInput
    {
        /// <summary>
        /// 组织单位Id(大于0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
    }
}