using System.Collections.Generic;
using Abp.Application.Services.Dto;
using Abp.Notifications;

namespace Vapps.Notifications.Dto
{
    public class GetNotificationsOutput : PagedResultDto<UserNotification>
    {
        /// <summary>
        /// 未读消息数量
        /// </summary>
        public int UnreadCount { get; set; }

        public GetNotificationsOutput(int totalCount, int unreadCount, List<UserNotification> notifications)
            : base(totalCount, notifications)
        {
            UnreadCount = unreadCount;
        }
    }
}