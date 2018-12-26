using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Vapps.Enums;

namespace Vapps.Authorization.Users.Profile.Dto
{
    [AutoMap(typeof(User))]
    public class CurrentUserProfileEditDto
    {
        /// <summary>
        /// 名字
        /// </summary>
        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 昵称
        /// </summary>
        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string NickName { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        [Required]
        [StringLength(User.MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        [Required]
        [StringLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// 时区
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// 性别
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// 头像Url
        /// </summary>
        public string ProfilePictureUrl { get; set; }

        /// <summary>
        /// 图片Id
        /// </summary>
        public int ProfilePictureId { get; set; }
    }
}