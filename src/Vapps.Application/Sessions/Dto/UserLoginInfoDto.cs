using Abp.Application.Services.Dto;

namespace Vapps.Sessions.Dto
{
    public class UserLoginInfoDto : EntityDto<long>
    {
        /// <summary>
        /// 名
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 姓
        /// </summary>
        public string Surname { get; set; }

        /// <summary>
        /// 用户名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 邮箱地址
        /// </summary>
        public string EmailAddress { get; set; }

        /// <summary>
        /// 头像Id
        /// </summary>
        public string ProfilePictureUrl { get; set; }

        /// <summary>
        /// 电话号码
        /// </summary>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnreadNotificationCount { get; set; }
    }
}
