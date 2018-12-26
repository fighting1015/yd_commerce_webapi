using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Vapps.Authorization.Permissions.Dto;

namespace Vapps.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        /// <summary>
        /// 角色详情
        /// </summary>
        public RoleEditDto Role { get; set; }

        /// <summary>
        /// 权限集合
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }

        /// <summary>
        /// 赋予权限名称集合
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}