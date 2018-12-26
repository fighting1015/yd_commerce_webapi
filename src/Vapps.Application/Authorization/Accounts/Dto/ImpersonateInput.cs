using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Accounts.Dto
{
    public class ImpersonateInput
    {
        /// <summary>
        /// 租户Id(可空)
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 用户Id(大于0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long UserId { get; set; }
    }
}