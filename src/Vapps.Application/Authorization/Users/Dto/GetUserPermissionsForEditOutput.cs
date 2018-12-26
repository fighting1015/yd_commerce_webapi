using System.Collections.Generic;
using Vapps.Authorization.Permissions.Dto;

namespace Vapps.Authorization.Users.Dto
{
    public class GetUserPermissionsForEditOutput
    {
        /// <summary>
        /// 现有权限
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }

        /// <summary>
        /// 授予权限名称
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}