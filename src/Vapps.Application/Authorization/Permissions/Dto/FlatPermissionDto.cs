using Abp.AutoMapper;

namespace Vapps.Authorization.Permissions.Dto
{
    [AutoMapFrom(typeof(Abp.Authorization.Permission))]
    public class FlatPermissionDto
    {
        /// <summary>
        /// 父权限名称
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// (系统)名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 显示名称
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 默认授予
        /// </summary>
        public bool IsGrantedByDefault { get; set; }
    }
}