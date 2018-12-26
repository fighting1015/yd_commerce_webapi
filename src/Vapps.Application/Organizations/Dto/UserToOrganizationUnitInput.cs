using System.ComponentModel.DataAnnotations;

namespace Vapps.Organizations.Dto
{
    public class UserToOrganizationUnitInput
    {
        /// <summary>
        /// �û� Id(����0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        /// <summary>
        /// ��֯��λ Id(����0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}