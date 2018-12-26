using System.ComponentModel.DataAnnotations;
using Abp.Auditing;

namespace Vapps.Authorization.Users.Profile.Dto
{
    public class ChangePasswordInput
    {
        /// <summary>
        /// ������
        /// </summary>
        [Required]
        [DisableAuditing]
        public string CurrentPassword { get; set; }

        /// <summary>
        /// ������
        /// </summary>
        [Required]
        [DisableAuditing]
        public string NewPassword { get; set; }
    }
}