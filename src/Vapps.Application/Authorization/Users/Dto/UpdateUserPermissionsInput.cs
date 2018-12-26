using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Users.Dto
{
    public class UpdateUserPermissionsInput
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        [Range(1, int.MaxValue)]
        public long Id { get; set; }

        /// <summary>
        /// 授予权限
        /// </summary>
        [Required]
        public List<string> GrantedPermissionNames { get; set; }
    }
}