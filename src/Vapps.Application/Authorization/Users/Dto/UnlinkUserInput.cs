using Abp;

namespace Vapps.Authorization.Users.Dto
{
    public class UnlinkUserInput
    {
        /// <summary>
        /// �⻧Id(�ɿ�)
        /// </summary>
        public int? TenantId { get; set; }

        /// <summary>
        /// �û�Id
        /// </summary>
        public long UserId { get; set; }

        public UserIdentifier ToUserIdentifier()
        {
            return new UserIdentifier(TenantId, UserId);
        }
    }
}