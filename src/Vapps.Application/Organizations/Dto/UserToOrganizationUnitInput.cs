using System.ComponentModel.DataAnnotations;

namespace Vapps.Organizations.Dto
{
    public class UserToOrganizationUnitInput
    {
        /// <summary>
        /// 用户 Id(大于0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }

        /// <summary>
        /// 组织单位 Id(大于0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long OrganizationUnitId { get; set; }
    }
}