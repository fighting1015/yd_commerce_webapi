using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;

namespace Vapps.Authorization.Roles.Dto
{
    [AutoMap(typeof(Role))]
    public class RoleEditDto
    {
        /// <summary>
        /// ��ɫId(�ɿ�)
        /// </summary>
        public int? Id { get; set; }

        /// <summary>
        /// ��ʾ��
        /// </summary>
        [Required]
        public string DisplayName { get; set; }
        
        /// <summary>
        /// �Ƿ�Ĭ��
        /// </summary>
        public bool IsDefault { get; set; }
    }
}