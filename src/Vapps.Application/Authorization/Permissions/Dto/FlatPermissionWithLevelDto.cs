namespace Vapps.Authorization.Permissions.Dto
{
    public class FlatPermissionWithLevelDto : FlatPermissionDto
    {
        /// <summary>
        /// 权限等级
        /// </summary>
        public int Level { get; set; }
    }
}
