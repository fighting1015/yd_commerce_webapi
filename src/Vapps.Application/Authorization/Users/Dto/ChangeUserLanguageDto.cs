using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Users.Dto
{
    public class ChangeUserLanguageDto
    {
        /// <summary>
        /// 语言名称
        /// </summary>
        [Required]
        public string LanguageName { get; set; }
    }
}
