using System.Collections.Generic;

namespace Vapps.Notifications.Dto
{
    public class GetNotificationSettingsOutput
    {
        /// <summary>
        /// 是否订阅
        /// </summary>
        public bool ReceiveNotifications { get; set; }

        /// <summary>
        /// 订阅消息
        /// </summary>
        public List<NotificationSubscriptionWithDisplayNameDto> Notifications { get; set; }
    }
}