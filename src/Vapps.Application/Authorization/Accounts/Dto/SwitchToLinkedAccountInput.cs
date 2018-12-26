using System.ComponentModel.DataAnnotations;
using Abp;

namespace Vapps.Authorization.Accounts.Dto
{
    public class SwitchToLinkedAccountInput
    {
        /// <summary>
        /// 租户Id(可空)
        /// </summary>
        public int? TargetTenantId { get; set; }

        /// <summary>
        /// 目标用户Id(大于0)
        /// </summary>
        [Range(1, long.MaxValue)]
        public long TargetUserId { get; set; }

        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TargetTenantId, TargetUserId);
        }
    }
}
