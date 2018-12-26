using Abp.Auditing;
using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Users.Dto
{
    public class LinkToUserInput
    {
        /// <summary>
        /// �⻧����
        /// </summary>
        public string TenancyName { get; set; }

        /// <summary>
        /// �û����������ַ
        /// </summary>
        [Required]
        public string UsernameOrEmailAddress { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        [Required]
        [DisableAuditing]
        public string Password { get; set; }
    }
}