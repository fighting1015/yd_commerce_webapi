using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Roles.Dto
{
    public class CreateOrUpdateRoleInput
    {
        /// <summary>
        /// 角色详情
        /// </summary>
        [Required]
        public RoleEditDto Role { get; set; }

        /// <summary>
        /// 授予权限
        /// </summary>
        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}