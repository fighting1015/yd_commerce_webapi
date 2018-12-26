using System.ComponentModel.DataAnnotations;

namespace Vapps.Authorization.Users.Profile.Dto
{
    public class UpdateProfilePictureInput
    {
        /// <summary>
        /// 图片Id
        /// </summary>
        public int ProfilePictureId { get; set; }
    }
}