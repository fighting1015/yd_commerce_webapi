using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace Vapps.Authorization.Users.Profile.Dto
{
    public class ChangePasswordInput
    {
        /// <summary>
        /// ¾ÉÃÜÂë
        /// </summary>
        [Required]
        [DisableAuditing]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// ĞÂÃÜÂë
        /// </summary>
        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}