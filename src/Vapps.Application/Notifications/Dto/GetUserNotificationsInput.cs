using Abp.Notifications;
using Vapps.Dto;

namespace Vapps.Notifications.Dto
{
    public class GetUserNotificationsInput : PagedInputDto
    {
        /// <summary>
        /// 通知状态（可空）
        /// </summary>
        public UserNotificationState? State { get; set; }
    }
}