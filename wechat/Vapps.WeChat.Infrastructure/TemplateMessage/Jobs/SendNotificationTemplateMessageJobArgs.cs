using System;
using Vapps.Notifications;

namespace Vapps.WeChat.TemplateMessage.Job
{
    [Serializable]
    public class SendNotificationTemplateMessageJobArgs
    {
        public NotificationType EventType { get; set; }
    }
}
