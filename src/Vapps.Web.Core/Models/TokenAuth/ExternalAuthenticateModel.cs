using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace Vapps.Web.Models.TokenAuth
{
    public class ExternalAuthenticateModel
    {
        /// <summary>
        /// 认证类型
        /// </summary>
        [Required]
        [MaxLength(UserLogin.MaxLoginProviderLength)]
        public string AuthProvider { get; set; }

        /// <summary>
        /// 认证秘钥
        /// </summary>
        [Required]
        [MaxLength(UserLogin.MaxProviderKeyLength)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// 认证访问码
        /// </summary>
        [Required]
        public string ProviderAccessCode { get; set; }

        /// <summary>
        /// 是否单步登陆
        /// </summary>
        public bool? SingleSignIn { get; set; }

    }
}