using System.Collections.Generic;

namespace Vapps.Notifications.Dto
{
    public class UpdateNotificationSettingsInput
    {
        /// <summary>
        /// 是否接受通知
        /// </summary>
        public bool ReceiveNotifications { get; set; }

        /// <summary>
        /// 通知订阅
        /// </summary>
        public List<NotificationSubscriptionDto> Notifications { get; set; }
    }
}