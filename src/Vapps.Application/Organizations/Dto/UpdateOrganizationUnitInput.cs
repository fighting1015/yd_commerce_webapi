using System.ComponentModel.DataAnnotations;
using Abp.Organizations;

namespace Vapps.Organizations.Dto
{
    public class UpdateOrganizationUnitInput
    {
        /// <summary>
        /// ��֯��λId(����0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        [Required]
        [StringLength(OrganizationUnit.MaxDisplayNameLength)]
        public string DisplayName { get; set; }
    }
}