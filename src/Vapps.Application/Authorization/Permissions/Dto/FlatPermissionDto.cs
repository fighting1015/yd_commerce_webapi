using Abp.AutoMapper;

namespace Vapps.Authorization.Permissions.Dto
{
    [AutoMapFrom(typeof(Abp.Authorization.Permission))]
    public class FlatPermissionDto
    {
        /// <summary>
        /// ��Ȩ������
        /// </summary>
        public string ParentName { get; set; }

        /// <summary>
        /// (ϵͳ)����
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// ��ʾ����
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Ĭ������
        /// </summary>
        public bool IsGrantedByDefault { get; set; }
    }
}