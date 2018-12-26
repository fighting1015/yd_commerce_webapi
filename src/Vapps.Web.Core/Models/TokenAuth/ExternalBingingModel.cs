using Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Web.Models.TokenAuth
{
    public class ExternalBindingModel
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
    }
}
