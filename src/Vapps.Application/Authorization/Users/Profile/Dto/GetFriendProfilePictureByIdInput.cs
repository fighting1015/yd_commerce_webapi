using System;

namespace Vapps.Authorization.Users.Profile.Dto
{
    /// <summary>
    /// 获取朋友资料
    /// </summary>
    public class GetFriendProfilePictureByIdInput
    {
        /// <summary>
        /// 图片Id
        /// </summary>
        public long ProfilePictureId { get; set; }

        /// <summary>
        /// 用户Id
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        public int? TenantId { get; set; }
    }
}
