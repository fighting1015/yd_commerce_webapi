using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Users.Dto
{
    public class CreateOrUpdateUserInput
    {
        /// <summary>
        /// 用户基本信息
        /// </summary>
        [Required]
        public UserEditDto User { get; set; }

        /// <summary>
        /// 分配角色名称
        /// </summary>
        [Required]
        public string[] AssignedRoleNames { get; set; }

        /// <summary>
        /// 授予权限
        /// </summary>
        public List<string> GrantedPermissionNames { get; set; }

        /// <summary>
        /// 发送激活邮件
        /// </summary>
        public bool SendActivationEmail { get; set; }

        /// <summary>
        /// 设置随机密码
        /// </summary>
        public bool SetRandomPassword { get; set; }
    }
}