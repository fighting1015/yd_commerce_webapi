using Abp;

namespace Vapps.Authorization.Users.Dto
{
    public class UnlinkUserInput
    {
        /// <summary>
        /// 租户Id(可空)
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TenantId, UserId);
        }
    }
}