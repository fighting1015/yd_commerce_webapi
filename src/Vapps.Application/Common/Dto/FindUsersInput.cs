using Vapps.Dto;

namespace Vapps.Common.Dto
{
    public class FindUsersInput : PagedAndFilteredInputDto
    {
        /// <summary>
        /// 租户Id
        /// </summary>
        public int? TenantId { get; set; }
    }
}