using System.ComponentModel.DataAnnotations;
using Abp.Authorization.Users;

namespace Vapps.Web.Models.TokenAuth
{
    public class ExternalAuthenticateModel
    {
        /// <summary>
        /// ��֤����
        /// </summary>
        [Required]
        [MaxLength(UserLogin.MaxLoginProviderLength)]
        public string AuthProvider { get; set; }

        /// <summary>
        /// ��֤��Կ
        /// </summary>
        [Required]
        [MaxLength(UserLogin.MaxProviderKeyLength)]
        public string ProviderKey { get; set; }

        /// <summary>
        /// ��֤������
        /// </summary>
        [Required]
        public string ProviderAccessCode { get; set; }

        /// <summary>
        /// �Ƿ񵥲���½
        /// </summary>
        public bool? SingleSignIn { get; set; }

    }
}