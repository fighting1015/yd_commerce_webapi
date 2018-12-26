using Abp.Authorization.Users;
using Abp.Domain.Entities.Auditing;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.Authorization.Users
{
    [Table("UserLogins")]
    public class ExternalUserLogin : UserLogin, IHasCreationTime
    {
        /// <summary>
        /// 用户名
        /// </summary>
        [MaxLength(MaxLoginProviderLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 唯一凭证
        /// </summary>
        public string UnionProviderKey { get; set; }

        /// <summary>
        /// 显示名称/昵称 
        /// </summary>
        public string ExternalDisplayName { get; set; }

        /// <summary>
        /// 第三方接口调用凭证
        /// </summary>
        [MaxLength(MaxProviderKeyLength)]
        public string AccessToken { get; set; }

        /// <summary>
        /// 刷新凭证
        /// </summary>
        [MaxLength(MaxProviderKeyLength)]
        public string RefreshToken { get; set; }

        /// <summary>
        /// 调用凭证过期时间
        /// </summary>
        public DateTime AccessTokenOutDataTime { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreationTime { get; set; }
    }
}
