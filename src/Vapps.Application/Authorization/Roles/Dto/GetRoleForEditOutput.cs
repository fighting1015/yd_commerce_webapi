using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Vapps.Authorization.Permissions.Dto;

namespace Vapps.Authorization.Roles.Dto
{
    public class GetRoleForEditOutput
    {
        /// <summary>
        /// ��ɫ����
        /// </summary>
        public RoleEditDto Role { get; set; }

        /// <summary>
        /// Ȩ�޼���
        /// </summary>
        public List<FlatPermissionDto> Permissions { get; set; }

        /// <summary>
        /// ����Ȩ�����Ƽ���
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }
    }
}