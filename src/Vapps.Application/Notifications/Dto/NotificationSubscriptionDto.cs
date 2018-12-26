using System.ComponentModel.DataAnnotations;
using Abp.Notifications;

namespace Vapps.Notifications.Dto
{
    /// <summary>
    /// 消息订阅
    /// </summary>
    public class NotificationSubscriptionDto
    {
        /// <summary>
        /// 消息名称
        /// </summary>
        [Required]
        [MaxLength(NotificationInfo.MaxNotificationNameLength)]
        public string Name { get; set; }

        /// <summary>
        /// 是否订阅
        /// </summary>
        public bool IsSubscribed { get; set; }
    }
}