using Abp.AutoMapper;
using Abp.Notifications;

namespace Vapps.Notifications.Dto
{
    [AutoMapFrom(typeof(NotificationDefinition))]
    public class NotificationSubscriptionWithDisplayNameDto : NotificationSubscriptionDto
    {
        /// <summary>
        /// ��ʾ����
        /// </summary>
        public string DisplayName { get; set; }

        /// <summary>
        /// ����
        /// </summary>
        public string Description { get; set; }
    }
}