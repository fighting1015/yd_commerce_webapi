using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Vapps.Authorization.Roles.Dto
{
    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        /// <summary>
        /// 角色Id(可空)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// 显示名
        /// </summary>
        [Required]
        public string DisplayName { get; set; }
        
        /// <summary>
        /// 是否默认
        /// </summary>
        public bool IsDefault { get; set; }
    }
}