using Abp.Authorization.Users;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Vapps.Authorization.Users
{
    [Table("AbpUserLoginAttempts")]
    public class LoginAttempt : UserLoginAttempt
    {
        /// <summary>
        /// Maximum length of <see cref="BrowserInfo"/> property.
        /// </summary>
        public const int NewMaxBrowserInfoLength = 512;


        //
        // 摘要:
        //     Browser information if this method is called in a web request.
        [Required]
        [MaxLength(512)]
        public override string BrowserInfo { get; set; }
    }
}
