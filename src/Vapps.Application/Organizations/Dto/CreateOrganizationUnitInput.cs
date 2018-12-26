using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace Vapps.Organizations.Dto
{
    public class CreateOrganizationUnitInput
    {
        /// <summary>
        /// 父节点Id(可空)
        /// </summary>
        public long? ParentId { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
    }
}