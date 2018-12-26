using System;

namespace Vapps.Authorization.Users.Dto
{
    public class GetUserForEditOutput
    {
        /// <summary>
        /// 头像图片Id
        /// </summary>
        public long ProfilePictureId { get; set; }

        /// <summary>
        /// 用户资料
        /// </summary>
        public UserEditDto User { get; set; }

        /// <summary>
        /// 角色
        /// </summary>
        public UserRoleDto[] Roles { get; set; }

        /// <summary>
        /// 权限
        /// </summary>
        public GetUserPermissionsForEditOutput Permissions { get; set; }

        /// <summary>
        /// 第三方登陆绑定记录
        /// </summary>
        public ExternalUserLoginDto[] ExternalLogins { get; set; }
    }
}