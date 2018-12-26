using Abp.AutoMapper;
using Abp.Notifications;

namespace Vapps.Notifications.Dto
{
    [AutoMapFrom(typeof(NotificationDefinition))]
    public class NotificationSubscriptionWithDisplayNameDto : NotificationSubscriptionDto
    {
        /// <summary>
        /// œ‘ æ√˚≥∆
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// √Ë ˆ
        /// </summary>
        public string Description { get; set; }
    }
}