namespace Vapps.Authorization.Users.Dto
{
    /// <summary>
    /// 用户角色
    /// </summary>
    public class UserRoleDto
    {
        /// <summary>
        /// 角色Id/唯一凭证
        /// </summary>
        public int RoleId { get; set; }

        /// <summary>
        /// 角色名/系统名
        /// </summary>
        public string RoleName { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string RoleDisplayName { get; set; }

        /// <summary>
        /// 是否默认分配角色
        /// </summary>
        public bool IsAssigned { get; set; }
    }
}