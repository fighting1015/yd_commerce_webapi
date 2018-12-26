using System.ComponentModel.DataAnnotations;
using Abp.AutoMapper;
using Vapps.Enums;

namespace Vapps.Authorization.Users.Profile.Dto
{
    [AutoMap(typeof(User))]
    public class CurrentUserProfileEditDto
    {
        /// <summary>
        /// ����
        /// </summary>
        [Required]
        [StringLength(User.MaxNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// �ǳ�
        /// </summary>
        [Required]
        [StringLength(User.MaxSurnameLength)]
        public string NickName { get; set; }

        /// <summary>
        /// �û���
        /// </summary>
        [Required]
        [StringLength(User.MaxUserNameLength)]
        public string UserName { get; set; }

        /// <summary>
        /// �����ַ
        /// </summary>
        [Required]
        [StringLength(User.MaxEmailAddressLength)]
        public string EmailAddress { get; set; }

        public string PhoneNumber { get; set; }

        /// <summary>
        /// ʱ��
        /// </summary>
        public string Timezone { get; set; }

        /// <summary>
        /// �Ա�
        /// </summary>
        public GenderType Gender { get; set; }

        /// <summary>
        /// ͷ��Url
        /// </summary>
        public string ProfilePictureUrl { get; set; }

        /// <summary>
        /// ͼƬId
        /// </summary>
        public int ProfilePictureId { get; set; }
    }
}